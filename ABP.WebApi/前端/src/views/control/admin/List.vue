<template>
  <a-card :bordered="false">
    <!--搜索-->
    <div class="table-page-search-wrapper">
      <a-row type="flex" justify="space-around" :gutter="48">
        <a-col :md="18" :sm="24">
          <a-button v-action:create type="primary" @click="$refs.add.show()" icon="plus" class="right10">新增</a-button>
          <!-- <a-radio-group :defaultValue="0" v-model="queryParam.isDeleted" buttonStyle="solid" class="right10" @change="search">
            <a-radio-button :value="0">正常</a-radio-button>
            <a-radio-button :value="1">已删除</a-radio-button>
          </a-radio-group>
          <a-select v-model="queryParam.status" style="width:120px;" class="right10" @change="search">
            <a-select-option value="-1">管理员状态</a-select-option>
            <a-select-option value="1">正常</a-select-option>
            <a-select-option value="2">禁用</a-select-option>
          </a-select> -->
        </a-col>
        <a-col :md="6" :sm="24">
          <div class="table-page-search-submitButtons" >
            <a-input-search
              placeholder="用户名"
              v-model="queryParam.vague"
              enterButton="查询"
              @search="search" >
            </a-input-search>
          </div>
        </a-col>
      </a-row>
    </div>

    <!--列表-->
    <a-table
      ref="table"
      rowKey="id"
      :pagination="table.pagination"
      :loading="table.loading"
      :columns="table.columns"
      :dataSource="table.dataSource"
      @change="loadDataing">
      <span slot="username" slot-scope="text, record">
        <!-- <a-badge :count="record.id" :overflowCount="9999999999999" :numberStyle="{backgroundColor: '#52c41a'} "/>
        <br> -->
        {{ record.userName }}
        <a-iconfont type="icon-jiesuo" v-show="checkLock(record.lockoutEnd)"/>
      </span>
      <span slot="telephone" slot-scope="text, record">
        <a-icon type="phone" fontSize="14" theme="twoTone" style="margin-right:6px;" v-show="record.phoneNumber" /> {{ record.phoneNumber }}
        <!-- <br>
        <a-icon type="mail" fontSize="14/" theme="twoTone" style=" margin-right:5px;" v-show="record.email"/> {{ record.email }} -->
      </span>
      <span slot="email" slot-scope="text, record">
        <!-- <a-icon type="phone" fontSize="14" theme="twoTone" style="margin-right:6px;" v-show="record.telephone" /> {{ record.telephone }}
        <br> -->
        <a-icon type="mail" fontSize="14/" theme="twoTone" style=" margin-right:5px;" v-show="record.email"/> {{ record.email }}
      </span>
      <!-- <span slot="roles" slot-scope="text, record">
        <a-tag v-if="record.isSystem" color="cyan"> 超级管理员</a-tag>
        <a-tag v-else v-for="role in text" :key="role" color="cyan">  {{ role }}</a-tag>
      </span>
      <span slot="createTime" slot-scope="text">{{ text | dayFormat('YYYY-MM-DD HH:mm:ss') }}</span>
      <span slot="loginTime" slot-scope="text">{{ text | dayFormat('YYYY-MM-DD HH:mm:ss') }}</span> -->
      <span slot="status" slot-scope="text">
        <a-badge v-if="text" status="success" text="是"/>
        <a-badge v-else status="error" text="否"/>
      </span>
      <span slot="action" slot-scope="text, record">
        <template>
          <!-- <a v-action:details @click="$refs.detail.show(record.id,record.roles)">详情</a> -->
          <a-divider v-show="queryParam.isDeleted===0" type="vertical" />
          <a-dropdown v-show="queryParam.isDeleted===0">
            <a class="ant-dropdown-link">更多
              <a-icon type="down" />
            </a>
            <a-menu slot="overlay" >
              <a-menu-item v-action:modify :disabled="record.userName==='admin'" @click="$refs.edit.show(record.id)" >
                编辑
              </a-menu-item>
              <a-menu-item v-action:modify :disabled="record.userName==='admin'" @click="$refs.addrole.show(record.id)" >
                分配角色
              </a-menu-item>
              <!-- <a-menu-item v-show="checkLock(record.lockoutEnd)" v-action:modify_status :disabled="record.isSystem" @click="handleModifyStatus(record.id,record.username,record.status)" >
                <span>解除锁定</span>
              </a-menu-item> -->
              <a-menu-item v-action:delete :disabled="record.userName==='admin'" @click="handleDelete(record.id,record.userName)" >
                删除
              </a-menu-item>
              <a-menu-item v-action:reset_password :disabled="record.userName==='admin'" @click="$refs.reset.show(record.id,record.userName)" >
                重置密码
              </a-menu-item>
            </a-menu>
          </a-dropdown>
        </template>
      </span>
    </a-table>

    <!--功能模块-->
    <admin-detail ref="detail" ></admin-detail>
    <admin-add-role ref="addrole"></admin-add-role>
    <admin-edit ref="edit" @complete="loadDataing"></admin-edit>
    <admin-add ref="add" @complete="loadDataing"></admin-add>
    <admin-reset-password ref="reset" @complete="loadDataing"></admin-reset-password>
  </a-card>
</template>

<script>
import AdminEdit from './Edit'
import AdminAdd from './Add'
import AdminAddRole from './AddRole'
import AdminDetail from './Detail'
import { GetAllUsers, deleteUser } from '@/api/auth'
import AdminResetPassword from './ResetPassword'
// import { getAccountList, modifyStatusAccount, deleteAccount } from '@/api/control'
export default {
  components: {
    AdminEdit, AdminAdd, AdminResetPassword, AdminDetail, AdminAddRole
  },
  data () {
    return {
      description: '列表使用场景：后台管理中的权限管理以及角色管理，可用于基于 RBAC 设计的角色权限控制，颗粒度细到每一个操作类型。',
      // 查询参数
      queryParam: {
        vague: '',
        status: '-1',
        isDeleted: 0,
        pagedCount: 10,
        skipPage: 1
      },
      table: {
        loading: false,
        dataSource: [],
        pagination: {
          showTotal: (total) => `总计 ${total} 条`,
          hideOnSinglePage: true,
          showSizeChanger: true,
          current: 1,
          total: 0
        },
        // 表头
        columns: [
          {
            title: '用户名',
            dataIndex: 'userName',
            scopedSlots: {
              customRender: 'username'
            }
          },
          {
            title: '邮箱',
            dataIndex: 'email',
            scopedSlots: {
              customRender: 'email'
            }
          },
          {
            title: '邮箱是否确认',
            align: 'center',
            dataIndex: 'emailConfirmed',
            width: 60,
            scopedSlots: {
              customRender: 'status'
            }
          },
          {
            title: '手机',
            dataIndex: 'phoneNumber',
            scopedSlots: {
              customRender: 'telephone'
            }
          },
          {
            title: '启用锁定',
            width: 60,
            dataIndex: 'lockoutEnabled',
            scopedSlots: {
              customRender: 'status'
            }
          },
          {
            title: '两步认证',
            width: 60,
            dataIndex: 'twoFactorEnabled',
            scopedSlots: {
              customRender: 'status'
            }
          },
          {
            title: '登录失败次数',
            dataIndex: 'accessFailedCount',
            width: 120,
            align: 'center'
          },
          {
            title: '操作',
            align: 'center',
            width: 150,
            dataIndex: 'action',
            scopedSlots: {
              customRender: 'action'
            }
          }
        ]
      }
    }
  },
  created () {
    this.loadDataing()
  },
  watch: {
    'queryParam.skipPage' () {
      this.table.pagination.current = this.queryParam.skipPage
    }
  },
  methods: {
    /**
    *搜索 查询列表
    */
    search () {
      this.queryParam.skipPage = 1
      this.loadDataing()
    },
    /**
       * 加载数据方法
       * @param {Object} pagination 分页选项器
       * @param {Object} filters 过滤条件
       * @param {Object} sorter 排序条件
       */
    loadDataing (pagination, filters, sorter) {
      this.table.loading = true
      if (pagination) {
        this.queryParam.skipPage = pagination.current
        this.queryParam.pagedCount = pagination.pageSize
      }
      GetAllUsers({
        'searchText': this.queryParam.vague,
        'page': this.queryParam.skipPage,
        'pageSize': this.queryParam.pagedCount
      }).then(res => {
        this.table.loading = false
        if (res.users) {
          this.table.pagination.total = res.totalCount
          this.table.dataSource = res.users
        } else {
          this.$notification.error({ message: '加载失败', description: res.message })
        }
      }).catch(() => {
        this.table.loading = false
      })
    },
    /**
     * 删除 管理员
     *@param {String} id 编号
     *@param {String} name 名称
     */
    handleDelete (id, name) {
      const _this = this
      this.$confirm({ title: '删除管理员',
        content: `是否删除 ${name} 管理员？`,
        onOk: () => {
          deleteUser(id).then(res => {
            this.loadDataing()
            _this.$message.success(`删除 ${name} 管理员成功`)
          }).catch(er => {
            _this.$message.error('删除失败;' + '请检查')
          })
        }
      })
    },
    checkLock (lockend) {
      if (lockend) {
        const nowdate = new Date()
        const locktime = this.$moment(lockend).utc().zone(-8).format('YYYY-MM-DD HH:mm:ss')
        const endtime = new Date(locktime)
        if (endtime.getTime() > nowdate.getTime()) {
          return true
        }
      }
      return false
    },
    /**
     * 禁用/启用管理员
     * @param {String} id 编号
     * @param {String} name 名称
     * @param {Number} status 状态
     */
    handleModifyStatus (id, name, status) {
      const _this = this
      var confirmTitle = status === 1 ? '禁用管理员？' : '启用管理员？'
      var confirmContent = status === 1 ? `禁用 ${name} 管理员` : `启用 ${name} 管理员`
      var reqData = {
        id: id,
        status: status === 1 ? 2 : 1
      }
      this.$confirm({ title: confirmTitle,
        content: `是否${confirmContent} ？`,
        onOk: () => {
          // modifyStatusAccount(reqData).then(res => {
          //   if (res.result.status === 200) {
          //     this.loadDataing()
          //     _this.$message.success(`${confirmContent}成功`)
          //   } else {
          //     _this.$message.error(`${confirmContent}失败`, res.result.message)
          //   }
          // })
        }
      })
    }
  }
}

</script>

<style lang="less" scoped>
.ant-avatar-lg {
  width: 48px;
  height: 48px;
  line-height: 48px;
}
.right10 {
   margin-right:10px;
}
.list-content-item {
  color: rgba(0, 0, 0, 0.45);
  display: inline-block;
  vertical-align: middle;
  font-size: 14px;
  margin-left: 40px;
  span {
    line-height: 20px;
  }
  p {
    margin-top: 4px;
    margin-bottom: 0;
    line-height: 22px;
  }
}
</style>
