<template>
  <s-modal
    title="编辑管理员"
    ref="modal"
    :maskClosable="false"
    :destroyOnClose="true"
    :width="500"
    :confirmLoading="confirmLoading"
    :visible="visible"
    @ok="submit"
    @cancel="close">
    <a-spin :spinning="confirmLoading">
      <a-form :form="form">
        <a-form-item label="用户名" v-bind="layout">
          <a-input placeholder="请输入用于登录的用户名" v-decorator="formDecorator.userName" />
        </a-form-item>
        <a-form-item label="手机号" v-bind="layout">
          <a-input placeholder="请输入手机号" v-decorator="formDecorator.phoneNumber" />
        </a-form-item>
        <a-form-item label="邮箱" v-bind="layout">
          <a-input placeholder="请输入邮箱" v-decorator="formDecorator.email" />
        </a-form-item>
      </a-form>
    </a-spin>
  </s-modal>
</template>

<script>
import { getRoleSimpleList, modifyAccount, getAccountDetail } from '@/api/control'
import { SModal } from '@/components'
import pick from 'lodash.pick'
import {
  GetUser,
  UpdateUser
} from '@/api/auth'
export default {
  name: 'UserModal',
  data () {
    return {
      visible: false,
      confirmLoading: false,
      form: this.$form.createForm(this),
      // 账户ID
      accountId: 0,
      userData: null,
      // 表单描述
      formDecorator: {
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
        userName: ['userName', {
          rules: [
            { required: true, message: '用户名不能为空' },
            { min: 2, message: '用户名字符长度不能少于 2 个字符' },
            { max: 16, message: '用户名字符长度不能大于 16 个字符' },
            { validator: this.CheckUsename }
          ]
        }]
      },
      // 布局
      layout: {
        labelCol: {
          xs: { span: 5 }
        },
        wrapperCol: {
          xs: { span: 18 }
        }
      },
      // 角色集合
      roleList: []
    }
  },
  created () {
    // this.loadRoleList()
  },
  components: {
    SModal
  },
  methods: {
    /**
     * 关闭当前窗口
     */
    close () {
      this.confirmLoading = false
      this.visible = false
    },
    /**
     * 显示添加对话框
     * @param {Number} id 管理账户ID
     */
    show (id) {
      this.visible = true
      this.loadAccountDetail(id)
    },
    /**
     *提交后端
     */
    submit () {
      this.form.validateFields((err, values) => {
        if (err) {
          return
        }
        // values.accountId = this.accountId

        this.confirmLoading = true
        this.userData.userName = values.userName
        this.userData.phoneNumber = values.phoneNumber
        this.userData.email = values.email
        UpdateUser(this.userData).then(res => {
          this.confirmLoading = false
          this.$refs.modal.success(`修改 ${values.userName} 账号成功`)
          this.$emit('complete')
        }).catch(er => {
          this.$refs.modal.error('修改账号信息失败', '请检查账号名称,邮箱是否已被占用')
          this.confirmLoading = false
          // this.close()
        })
        // modifyAccount(values).then(res => {
        //   this.confirmLoading = false
        //   if (res.status === 200) {
        //     this.$refs.modal.success(`修改 ${values.username} 管理员账户成功`)
        //     this.$emit('complete')
        //   } else {
        //     this.$refs.modal.error('修改管理员账户失败', res.message)
        //   }
        // }).catch(() => { this.close() })
      })
    },
    /**
     * 加载管理账户信息
     * @param {Number} id 管理账户ID
     */
    loadAccountDetail (id) {
      this.confirmLoading = true
      GetUser(id).then(res => {
        this.confirmLoading = false
        this.userData = res
        this.form.setFieldsValue(pick(res, 'userName', 'phoneNumber', 'email'))
        // if (res.status === 200) {
        //   this.accountId = res.result.id
        //   this.$nextTick(() => {
        //     this.form.setFieldsValue(pick(res.result, 'username', 'phoneNumber', 'email'))
        //   })
        // } else {
        //   this.$notification.error({ message: '获取管理账户详情失败', description: res.message })
        // }
      }).catch(() => {
        this.close()
        this.$notification.error({ message: '获取账户详情失败', description: '请检查' })
      })
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
    /**
     *加载角色数据
     */
    loadRoleList () {
      getRoleSimpleList().then(res => {
        if (res.result.status === 200) {
          this.roleList = res.result.result
        } else {
          this.$notification.error({ message: '获取角色失败', description: res.result.message })
        }
      })
    }

  }
}
</script>
