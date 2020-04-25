<template>
  <a-card :bordered="false">
    <a-spin :spinning="loading">
      <description-list title="菜单信息" >
        <description-list-item term="菜单标识">
          <a-tag color="#108ee9">{{ menuDetails.id }}</a-tag>
        </description-list-item>
        <description-list-item term="菜单名称">
          <a-icon v-show="menuDetails.icon" :type="menuDetails.icon" />
          {{ menuDetails.name }}</description-list-item>
        <description-list-item term="菜单状态">
          <a-badge v-if="menuDetails.enable" status="success" text="启用"/>
          <a-badge v-else status="error" text="禁用"/>
        </description-list-item>
        <description-list-item term="是否隐藏">
          <a-badge v-if="!menuDetails.hidden" status="success" text="显示"/>
          <a-badge v-else status="error" text="隐藏"/>
        </description-list-item>
        <description-list-item term="父级菜单">
          <a-tag color="pink"> {{ menuDetails.parentId===''?'顶级菜单':menuDetails.parentId }}</a-tag>
        </description-list-item>
        <description-list-item term="创建时间">
          {{ menuDetails.createTime | dayFormat('YYYY-MM-DD HH:mm') }}
        </description-list-item>
        <description-list-item v-show="menuDetails.redirect" term="跳转地址">
          <a :href="menuDetails.redirect" target="_blank">  <a-icon type="link" />
            {{ menuDetails.redirect }}
          </a>
        </description-list-item>
        <description-list-item term="描述">
          {{ menuDetails.description }}
        </description-list-item>
      </description-list>

      <!--操作功能列表-->
      <div class="table-page-search-wrapper" style="margin-bottom: 10px; text-align: right;">
        <a-button v-action:action_create type="primary" @click="$refs.add.show( menuDetails.id, menuDetails.name )" icon="plus" class="right10">新增功能</a-button>
      </div>
      <a-table
        ref="table"
        size="middle"
        :columns="table.columns"
        :dataSource="table.dataSource"
        :pagination="false">
        <span slot="name" slot-scope="text,record">
          {{ text }} <a-tag color="blue">{{ record.code }}</a-tag>
        </span>
        <span slot="isLogger" slot-scope="text">
          <a-badge v-if="text" status="success" text="记录"/>
          <a-badge v-else status="error" text="不记录"/>
        </span>
        <span slot="enable" slot-scope="text">
          <a-badge v-if="text" status="success" text="启用"/>
          <a-badge v-else status="error" text="禁用"/>
        </span>
        <!-- <span slot="createTime" slot-scope="text">
          {{ text*1000 | moment('YYYY-MM-DD HH:mm') }}
        </span> -->
        <span slot="createTime" slot-scope="text">{{ text | dayFormat('YYYY-MM-DD HH:mm:ss') }}</span>
        <span slot="apiTemplate" slot-scope="text, record">
          <a-tag v-for="item in record.httpMethods" :key="item" color="#87d068">{{ item }}</a-tag>
          {{ text }}
        </span>
        <span slot="sort" slot-scope="text, record">
          <modify-sort :text="text" :id="record.id" @complete="loadActionsData" type="action"></modify-sort>
        </span>
        <span slot="action" slot-scope="text, record, index">
          <a v-action:action_modify @click="$refs.edit.show(record, menuDetails.id, menuDetails.name )" v-if="!record.isSystem">编辑</a>
          <a v-action:action_modify v-else disabled>编辑</a>
          <a-divider type="vertical"/>
          <a v-action:action_delete @click="handleDelete(record.id, index)" v-if="!record.isSystem">删除</a>
          <a v-action:action_delete v-else disabled>删除</a>
        </span>
      </a-table>
    </a-spin>

    <action-add ref="add" @complete="loadActionsData"></action-add>
    <action-edit ref="edit" @complete="loadActionsData"></action-edit>
  </a-card>
</template>

<script>
import ModifySort from './modules/ModifySort'
import ActionAdd from './action/Add'
import ActionEdit from './action/Edit'
import { DescriptionList } from '@/components'
import { getMenuDetails, getActionList, deleteAction } from '@/api/control'
const DescriptionListItem = DescriptionList.Item
export default {
  name: 'MenuDetalis',
  components: { DescriptionList, DescriptionListItem, ModifySort, ActionAdd, ActionEdit },
  activated () {
    this.muenId = this.$route.query.data
    this.loadMenuData()
  },
  data () {
    return {
      loading: false,
      form: this.$form.createForm(this),

      muenId: 0,
      menuDetails: {}, // 菜单详情
      // 操作表结构
      table: {
        dataSource: [],
        columns: ActionColumns
      }
    }
  },
  methods: {
    /**
     * 加载菜单数据
     *
     */
    loadMenuData () {
      this.loading = true
      Promise.all([
        getMenuDetails(this.muenId),
        getActionList({ paramId: this.muenId })
      ])
        .then(res => {
          console.log(res[0])
          this.loading = false
          const menuRes = res[0]
          // 获取菜单详情
          if (menuRes.result.status === 200) {
            this.menuDetails = menuRes.result.result
          } else {
            this.$notification.error({ message: `加载 ${this.muenId} 菜单详情失败`, description: menuRes.result.message })
          }
          // 获取操作功能集合
          const actionsRes = res[1]
          if (actionsRes.result.status === 200) {
            this.table.dataSource = actionsRes.result.result.map(d => { d.key = d.id; return d })
          } else {
            this.$notification.error({ message: `加载 ${this.muenId} 菜单拥有操作功能失败`, description: actionsRes.result.message })
          }
        })
        .catch(() => {
          this.loading = false
        })
    },
    /**
     * 加载菜单数据
     */
    loadActionsData () {
      this.loading = true
      getActionList({ paramId: this.muenId }).then(res => {
        this.loading = false
        if (res.result.status === 200) {
          this.table.dataSource = res.result.result.map(d => { d.key = d.id; return d })
        } else {
          this.$notification.error({ message: `加载 ${this.muenId} 菜单拥有操作功能失败`, description: res.result.message })
        }
      }).catch(() => {
        this.loading = false
      })
    },
    /**
     * 删除操作功能
     * @param {String} id 功能标识
     * @param {Number} index 列表下标
     */
    handleDelete (id, index) {
      const _this = this
      this.$confirm({ title: '删除菜单',
        content: `是否删除 ${name} 菜单？`,
        onOk: () => {
          deleteAction(id).then(res => {
            if (res.result.status === 200) {
              _this.table.dataSource.splice(index, 1)
              _this.$message.success(`删除 ${id} 操作功能成功`)
            } else {
              _this.$message.error('删除操作功能失败;' + res.result.message)
            }
          })
        }
      })
    }
  }
}

/**
 * 操作功能列头
 */
const ActionColumns = [
  {
    title: '功能名称',
    dataIndex: 'name',
    scopedSlots: {
      customRender: 'name'
    }
  },
  {
    title: '状态',
    dataIndex: 'enable',
    scopedSlots: {
      customRender: 'enable'
    }
  },
  {
    title: '日志记录',
    dataIndex: 'isLogger',
    scopedSlots: {
      customRender: 'isLogger'
    }
  },
  {
    title: '添加时间',
    dataIndex: 'createTime',
    scopedSlots: {
      customRender: 'createTime'
    }
  },
  {
    title: 'API 地址',
    dataIndex: 'apiTemplate',
    scopedSlots: {
      customRender: 'apiTemplate'
    }
  },
  {
    title: '描述',
    dataIndex: 'description'
  },
  {
    title: '排序',
    dataIndex: 'sort',
    scopedSlots: {
      customRender: 'sort'
    }
  },
  {
    title: '操作',
    scopedSlots: {
      customRender: 'action'
    }
  }
]
</script>
