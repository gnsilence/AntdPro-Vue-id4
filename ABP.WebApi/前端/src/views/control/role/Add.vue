<template>
  <a-drawer
    title="新增角色"
    width="50%"
    :closable="false"
    @close="onClose"
    :destroyOnClose="true"
    :visible="visible">
    <a-spin :spinning="confirmLoading">
      <a-form :form="form">
        <a-form-item label="选择角色" v-bind="layout">
          <a-select showSearch placeholder="请给此管理员赋予角色" :allowClear="true" v-decorator="formDecorator.name">
            <a-select-option v-for="(role, index) in roleList" :key="index" :value="role.name">
              {{ role.name }}
            </a-select-option>
          </a-select>
        </a-form-item>
        <!-- <a-form-item v-bind="layout" label="角色名称" >
            <a-input placeholder="请输入角色名称" v-decorator="formDecorator.name" style="width:200px" />
          </a-form-item> -->
        <a-form-item v-bind="layout" label="角色描述">
          <a-textarea :rows="3" placeholder="请输入角色的描述" v-decorator="formDecorator.description" style="width:50%" />
        </a-form-item>
      </a-form>
    </a-spin>
    <a-divider> 分配权限</a-divider>
    <role-assign-permissions ref="assign"></role-assign-permissions>
    <div
      :style="{
        position: 'absolute',
        left: 0,
        bottom: 0,
        width: '100%',
        borderTop: '1px solid #e9e9e9',
        padding: '10px 16px',
        background: '#fff',
        textAlign: 'right',
      }">
      <a-button :style="{marginRight: '8px'}" @click="onClose">
        取消
      </a-button>
      <a-button @click="onSubmit" type="primary">确定</a-button>
    </div>
  </a-drawer>
</template>
<script>
import RoleAssignPermissions from './AssignPermissions'
import {
  GetAllRoles
} from '@/api/auth'
import {
  createRole,
  ifExistsRole
} from '@/api/control'
export default {
  data () {
    return {
      visible: false,
      roleList: [],
      checkrole: false,
      confirmLoading: false,
      form: this.$form.createForm(this),
      // 表单描述
      formDecorator: {
        name: ['name', {
          rules: [{
            required: true,
            message: '请给此管理员赋予角色'
          }
            // { max: 20, message: '角色名称字符长度不能大于 20 个字符' }
          ]
        }],
        roleId: ['roleId'],
        description: ['description', {
          rules: [{
            required: true,
            message: '角色描述不能为空'
          },
          {
            max: 200,
            message: '角色描述字符长度不能大于 200 个字符'
          }
          ]
        }]
      },
      // 布局
      layout: {
        labelCol: {
          xs: {
            span: 4
          }
        },
        wrapperCol: {
          xs: {
            span: 20
          }
        }
      }
    }
  },
  components: {
    RoleAssignPermissions
  },
  created () {
    this.getRoleList()
  },
  methods: {
    show () {
      this.visible = true
    },
    onClose () {
      this.visible = false
    },
    getRoleList () {
      GetAllRoles({
        'searchText': '',
        'page': 1,
        'pageSize': 999999
      }).then(res => {
        console.log(res)
        this.roleList = res.roles
      })
    },
    ifexistsrolebyname (name) {
      ifExistsRole(name).then(res => {
        this.checkrole = res.result
        if (this.checkrole) {
          this.$error({
            title: '创建角色失败',
            content: '所选角色已经存'
          })
          return false
        }
      })
    },
    /**
       *处理完成
       */
    complete () {
      this.onClose()
      this.$emit('complete')
    },
    /**
       *提交后端
       */
    onSubmit () {
      this.form.validateFields((err, values) => {
        if (err) {
          return
        }
        this.roleList.forEach(r => {
          if (r.name === values.name) {
            values.roleId = r.id
          }
        })
        console.log(values.roleId)
        values.permissions = []
        // values.name = values.roles
        ifExistsRole(values.name).then(res => {
          console.log(res.result)
          if (res.result === true) {
            this.$error({
              title: '创建角色失败',
              content: '所选角色已经存'
            })
          } else {
            Object.keys(this.$refs.assign.selectValue).forEach((key, index, arr) => {
              this.$refs.assign.selectValue[key].forEach(item => {
                values.permissions.push(item)
              })
            })
            if (values.permissions.length === 0) {
              this.$error({
                title: '创建角色失败',
                content: `必须给 ${values.name} 这个角色分配权限`
              })
              return
            }
            this.confirmLoading = true
            createRole(values).then(res => {
              console.log(res)
              this.confirmLoading = false
              if (res.result.status === 200) {
                this.$notification.success({
                  message: `创建 ${values.name} 角色成功`
                })
                this.complete()
              } else {
                this.$error({
                  title: '创建角色失败',
                  content: res.result.message
                })
              }
            }).catch(() => {
              this.onClose()
            })
          }
        })
      })
    }
  }
}
</script>
