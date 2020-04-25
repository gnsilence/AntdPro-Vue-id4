<template>
  <s-modal
    ref="modal"
    title="新增管理员"
    :maskClosable="false"
    :destroyOnClose="true"
    :width="500"
    :confirmLoading="confirmLoading"
    :visible="visible"
    @ok="submit"
    @cancel="close">
    <a-spin :spinning="confirmLoading">
      <a-form :form="form">
        <!-- <a-form-item label="昵称" v-bind="layout">
          <a-input placeholder="请输入昵称" v-decorator="formDecorator.name" style="width: 50%"/>
        </a-form-item> -->
        <a-form-item label="用户名" v-bind="layout">
          <a-input placeholder="请输入用于登录的用户名" v-decorator="formDecorator.username" />
        </a-form-item>
        <a-form-item label="手机号" v-bind="layout">
          <a-input placeholder="请输入手机号" v-decorator="formDecorator.phoneNumber" />
        </a-form-item>
        <a-form-item label="邮箱" v-bind="layout">
          <!-- <a-mentions
            placeholder="输入邮箱"
            :prefix="['@', '#']"
            @search="onSearch"
            @change="checkemail"
          >
            <a-mentions-option v-for="value in MOCK_DATA[prefix] || []" :key="value" :value="value">
              {{ value }}
            </a-mentions-option>
          </a-mentions> -->
          <a-input placeholder="请输入邮箱" v-decorator="formDecorator.email" />
        </a-form-item>
        <a-form-item label="登录密码" v-bind="layout">
          <a-input placeholder="请输入登录密码" type="password" v-decorator="formDecorator.password"/>
        </a-form-item>
        <a-form-item label="确认密码" v-bind="layout">
          <a-input placeholder="请再次输入登录密码进行确认" type="password" v-decorator="formDecorator.confirmPassword"/>
        </a-form-item>
        <a-divider/>
        <!-- <a-form-item label="赋予角色" v-bind="layout">
          <a-select placeholder="请给此管理员赋予角色" mode="multiple" :allowClear="true" v-decorator="formDecorator.roles">
            <a-select-option v-for="(role, index) in roleList" :key="index" :value="role.id">
              {{ role.name }}
            </a-select-option>
          </a-select>
        </a-form-item> -->
      </a-form>
    </a-spin>
  </s-modal>
</template>

<script>
// import md5 from 'md5'
import { SModal } from '@/components'
// import { mentions } from 'ant-design-vue'
import pick from 'lodash.pick'
import {
  GetAllRoles,
  addUser,
  changePassword,
  AddOrUpdateUserRole
} from '@/api/auth'
const MOCK_DATA = {
  '@': ['qq.com', '163.com', 'outlook.com', 'gmail.com', 'yahoo.com', 'msn.com', 'hotmail.com', 'aol.com', 'ask.com', 'live.com', '0355.net', '163.net', '263.net']
}
// import { getRoleSimpleList, createAccount } from '@/api/control'
export default {
  data () {
    return {
      prefix: '@',
      MOCK_DATA,
      visible: false,
      confirmLoading: false,
      form: this.$form.createForm(this),
      // 角色集合
      roleList: [],
      userid: '',
      // 表单描述
      formDecorator: {
        // name: ['name', {
        //   rules: [
        //     { required: true, message: '昵称不能为空' },
        //     { max: 20, message: '昵称字符长度不能大于 20 个字符' }
        //   ]
        // }],
        email: ['email', {
          rules: [
            { required: true, message: '邮箱不能为空' },
            // { min: 2, message: '手机号长度不能少于 11 个字符' },
            // { max: 11, message: '手机号长度不能大于 11 个字符' },
            { validator: this.checkEmail }
          ]
        }],
        phoneNumber: ['phoneNumber', {
          rules: [
            { required: true, message: '手机号不能为空' },
            { min: 2, message: '手机号长度不能少于 11 个字符' },
            { max: 11, message: '手机号长度不能大于 11 个字符' },
            { validator: this.checkPhoneNumber }
          ]
        }],
        username: ['username', {
          rules: [
            { required: true, message: '用户名不能为空' },
            { min: 2, message: '用户名字符长度不能少于 2 个字符' },
            { max: 16, message: '用户名字符长度不能大于 16 个字符' },
            { validator: this.CheckUsename }
          ]
        }],
        password: ['password', {
          rules: [
            { required: true, message: '登录密码不能为空' },
            { min: 2, message: '登录密码字符长度不能少于 2 个字符' },
            { max: 32, message: '登录密码字符长度不能大于 32 个字符' },
            { validator: this.PasswordStrong }
          ]
        }],
        confirmPassword: ['confirmPassword', {
          rules: [
            { required: true, message: '确认密码不能为空' },
            { validator: this.compareConfirmPassword }
          ]
        }]
        // roles: ['roles', {
        //   rules: [
        //     { required: true, message: '请给此管理员赋予角色' }
        //   ]
        // }]
      },
      // 布局
      layout: {
        labelCol: {
          xs: { span: 5 }
        },
        wrapperCol: {
          xs: { span: 18 }
        }
      }
    }
  },
  components: {
    SModal
  },
  created () {
    // this.loadRoleList()
  },
  methods: {
    onSearch (_, prefix) {
      console.log(_, prefix)
      this.prefix = prefix
      console.log(prefix)
    },
    checkemail (value) {
      this.form.setFieldsValue(pick({ email: value.replace(/\s+/g, '') }, 'email'))
    },
    /**
     *显示添加对话框
     */
    show () {
      this.visible = true
    },
    /**
     * 关闭当前窗口
     */
    close () {
      this.confirmLoading = false
      this.visible = false
    },
    /**
     *提交后端
     */
    submit () {
      this.form.validateFields((err, values) => {
        if (err) {
          return
        }
        console.log(values.email)
        const reg = /^[a-zA-Z0-9]+([-_.][a-zA-Z0-9]+)*@[a-zA-Z0-9]+([-_.][a-zA-Z0-9]+)*\.[a-z]{2,}$/
        if (values.email && values.email.match(reg)) {
        } else {
          this.$refs.modal.error('保存失败', '请输入正确的邮箱')
          return
        }
        // 登录需要MD5加密
        // values.password = md5(values.password)
        values.confirmPassword = ''

        this.confirmLoading = true
        const comfi = {
          userName: values.username,
          phoneNumber: values.phoneNumber,
          email: values.email
        }
        console.log(comfi)
        addUser(comfi).then(res => {
          console.log(res)
          if (res) {
            this.userid = res.id
            // 设置密码
            changePassword({
              userid: this.userid,
              password: values.password,
              confirmPassword: values.password
            }).then(res => {
              this.confirmLoading = false
              this.$refs.modal.success(`创建 ${values.username} 管理员账号成功`)
              this.$emit('complete')
              // 设置角色
              // const userroles = []
              // values.roles.forEach(k => {
              //   const role = {
              //     userId: this.userid,
              //     roleId: k
              //   }
              //   userroles.push(role)
              // })
              // AddOrUpdateUserRole(userroles).then(res => {
              //   this.confirmLoading = false
              //   this.$refs.modal.success(`创建 ${values.username} 管理员账号成功`)
              // }).catch(er => { this.confirmLoading = false })
            })
          }
        }).catch(er => {
          this.confirmLoading = false
          this.$refs.modal.error('创建管理员账号失败', '请检查账号名称,邮箱是否已被占用')
          // this.close()
        })
        // createAccount(values).then(res => {
        //   this.confirmLoading = false
        //   if (res.result.status === 200) {
        //     this.$refs.modal.success(`创建 ${values.username} 管理员账号成功`)
        //     this.$emit('complete')
        //   } else {
        //     this.$refs.modal.error('创建管理员账号失败', res.result.message)
        //   }
        // }).catch(() => { this.close() })
      })
    },
    /**
     *加载角色数据
     */
    loadRoleList () {
      this.confirmLoading = true
      GetAllRoles({
        'searchText': '',
        'page': 1,
        'pageSize': 999999
      }).then(res => {
        console.log(res)
        this.roleList = res.roles
        this.confirmLoading = false
      }).catch(() => { this.confirmLoading = false })
    },
    checkEmail (rule, value, callback) {
      const reg = /^[a-zA-Z0-9]+([-_.][a-zA-Z0-9]+)*@[a-zA-Z0-9]+([-_.][a-zA-Z0-9]+)*\.[a-z]{2,}$/
      if (value && value.match(reg)) {
        callback()
      } else {
        // eslint-disable-next-line standard/no-callback-literal
        callback('请输入正确的邮箱')
      }
    },
    checkPhoneNumber (rule, value, callback) {
      const reg = /^1[345789]\d{9}$/
      if (value && value.match(reg)) {
        callback()
      } else {
        // eslint-disable-next-line standard/no-callback-literal
        callback('请输入正确的手机号')
      }
    },
    CheckUsename (rule, value, callback) {
      const reg = /^[0-9a-zA-Z]*$/g
      if (value && value.match(reg)) {
        callback()
      } else {
        // eslint-disable-next-line standard/no-callback-literal
        callback('用户名只能是数字或字母')
      }
    },
    PasswordStrong (rule, value, callback) {
      const regs = '^(?![A-Za-z0-9]+$)(?![a-z0-9\\W]+$)(?![A-Za-z\\W]+$)(?![A-Z0-9\\W]+$)[a-zA-Z0-9\\W]{8,}$'
      // const form = this.form
      if (value && value.match(regs)) {
        callback()
      } else {
        // eslint-disable-next-line standard/no-callback-literal
        callback('请输入正确的密码,必须包含大小写字母数字和特殊符号')
      }
    },
    /**
     * 验证确认密码是否一致
     */
    compareConfirmPassword  (rule, value, callback) {
      const form = this.form
      if (value && value !== form.getFieldValue('password')) {
        // eslint-disable-next-line standard/no-callback-literal
        callback('您输入的两个密码不一致!')
      } else {
        callback()
      }
    }
  }
}
</script>
