<template>
  <a-drawer
    title="分配角色"
    width="60%"
    :closable="false"
    @close="onClose"
    :destroyOnClose="true"
    :visible="visible">
    <a-spin :spinning="confirmLoading">
      <a-form :form="form">
        <a-form-item label="选择角色" v-bind="layout">
          <a-select showSearch :disabled="disabledselect" placeholder="选择角色" :allowClear="true" v-decorator="formDecorator.roleId">
            <a-select-option v-for="(role, index) in roleList" :key="index" :value="role.roleId">
              {{ role.name }}
            </a-select-option>
          </a-select>
          <a-button
            :style="{
              position:'absolute',
              'margin-left': '30px',
              'margin-top': '3px'
            }"
            @click="onSubmit"
            :disabled="disabledselect"
            type="primary">分配角色</a-button>
        </a-form-item>
      </a-form>
    </a-spin>
    <a-divider>已分配角色列表</a-divider>
    <a-list class="demo-loadmore-list" itemLayout="horizontal" :dataSource="roledata">
      <a-list-item slot="renderItem" slot-scope="item, index">
        <a slot="actions" @click="deleteRole(item.id,item.name)">移除</a>
        <a-list-item-meta
          :description="item.description"
        >
          <a slot="title">{{ item.name }}</a>
          <a-avatar
            slot="avatar"
            src="https://zos.alipayobjects.com/rmsportal/ODTLcjxAfvqbxHnVXCYX.png"
          />
        </a-list-item-meta>
        <div>角色创建时间:{{ item.createTime }}</div>
      </a-list-item>
    </a-list>
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
      <!-- <a-button @click="onSubmit" type="primary">确定</a-button> -->
    </div>
  </a-drawer>
</template>
<script>
import { SModal } from '@/components'
import {
  AddOrUpdateUserRole,
  getUserRole,
  deleteUserRole
} from '@/api/auth'
import {
  ifExistsRole,
  getRoleList
} from '@/api/control'
export default {
  data () {
    return {
      visible: false,
      roleList: [],
      accountid: '',
      disabledselect: false,
      cuRoleId: '', // 当前选择的角色id
      roledata: [],
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
        roleId: ['roleId', { rules: [{
          required: true,
          message: '请选择分配的角色'
        }
        ] }]
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
            span: 10
          }
        }
      }
    }
  },
  components: {
    SModal
  },
  created () {

  },
  methods: {
    show (id) {
      this.accountid = id
      this.loadRoleList()
      this.visible = true
    },
    onClose () {
      this.visible = false
    },
    deleteRole (roleid, name) {
      this.$confirm({ title: '移除角色',
        content: `是否移除已经分配的 ${name} 角色？`,
        onOk: () => {
          const data = {
            userId: this.accountid, roleId: roleid
          }
          // console.log(data)
          deleteUserRole({ data: data }).then(res => {
            this.loadRoleList()
            this.$message.success(`移除${name} 角色成功`)
          }).catch(er => {
            this.$message.error('移除角色失败' + '请检查')
          })
        }
      })
    },
    getRoleList () {
      this.roledata = []
      getUserRole(this.accountid, `page=1&pageSize=999999`).then(res => {
        if (!res.roles.length) {
          this.roledata = []
          return
        }
        // this.roledata = res.roles
        res.roles.forEach(f => {
          for (let index = 0; index < this.roleList.length; index++) {
            if (this.roleList[index].name === f.name) {
              this.roledata.push({
                name: f.name,
                id: f.id,
                description: this.roleList[index].description,
                createTime: this.$moment(this.roleList[index].createTime).format('YYYY-MM-DD HH:MM')
              })
              this.roleList.splice(index, 1)// 移除已经分配的角色
            }
          }
        })
        if (this.roleList.length > 0) {
          this.disabledselect = false
        } else {
          this.disabledselect = true
        }
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
     *加载角色数据
     */
    loadRoleList () {
      this.roleList = []
      getRoleList({
        vague: '',
        enable: -1,
        pagedCount: 99999,
        skipPage: 1
      }).then(res => {
        if (res.result.status === 200) {
          this.roleList = res.result.result
          this.getRoleList()
        } else {
          this.$notification.error({ message: '获取角色失败', description: res.result.message })
        }
      })
    },
    /**
       *提交后端
       */
    onSubmit () {
      this.form.validateFields((err, values) => {
        if (!values.roleId) {
          this.$notification.error({ message: '分配角色失败', description: '暂无可分配角色' })
        }
        if (err) {
          return
        }
        this.confirmLoading = true
        // console.log(values.roleId)
        AddOrUpdateUserRole({
          userId: this.accountid,
          roleId: values.roleId
        }).then(res => {
          this.$notification.success({
            message: `分配角色成功`
          })
          this.roleList = []
          this.roledata = []
          this.loadRoleList()
          this.confirmLoading = false
        }).catch(er => {
          console.log(er)
          this.confirmLoading = false
          this.$notification.error({ message: '分配角色失败', description: '请检查' })
        })
        // values.name = values.roles
      })
    }
  }
}
</script>
