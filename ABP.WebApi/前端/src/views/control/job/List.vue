<template>
  <div>
    <a-card :bordered="false">
      <a-row>
        <a-col :sm="4" :xs="24">
          <head-info title="正在执行" :content="MorniApi.processingCount" :bordered="true"/>
        </a-col>
        <a-col :sm="4" :xs="24">
          <head-info title="成功数" :content="MorniApi.succeededListCount" :bordered="true"/>
        </a-col>
        <a-col :sm="4" :xs="24">
          <head-info title="失败数" :content="MorniApi.failedCount" :bordered="true"/>
        </a-col>
        <a-col :sm="4" :xs="24">
          <head-info title="已删除" :content="MorniApi.deletedJobs" :bordered="true"/>
        </a-col>
        <a-col :sm="4" :xs="24">
          <head-info title="队列数" :content="MorniApi.queues" :bordered="true"/>
        </a-col>
        <a-col :sm="4" :xs="24">
          <head-info title="等待中" :content="MorniApi.scheduledCount"/>
        </a-col>
      </a-row>
    </a-card>
    <a-card style="margin-top: 24px" :bordered="false">
      <!--搜索-->
      <div class="table-page-search-wrapper">
        <a-row type="flex" justify="space-around" :gutter="48">
          <a-col :md="18" :sm="24">
            <a-button type="primary" icon="plus" @click="$refs.add.show()" style="margin-right:10px" v-action:create>新增任务</a-button>
            <a-radio-group defaultValue="-1" v-model="queryParam.Enable" buttonStyle="solid" @change="search()">
              <a-radio-button value="-1">全部</a-radio-button>
              <a-radio-button value="1">启用</a-radio-button>
              <a-radio-button value="0">停用</a-radio-button>
            </a-radio-group>
          </a-col>
          <a-col :md="6" :sm="24">
            <span class="table-page-search-submitButtons" style="float:right">
              <a-input-search placeholder="任务名称,队列名称,http方法" v-model="queryParam.vague" enterButton="查询" @search="search()" style="width:300px;margin-right:10px">
              </a-input-search>
            </span>
          </a-col>
        </a-row>
      </div>
      <!--列表-->
      <a-table
        ref="table"
        rowKey="jobName"
        :pagination="tables.pagination"
        :columns="tables.columns"
        :loading="tables.loading"
        :dataSource="tables.dataSource"
        @change="loadDataing">
        <span slot="enable" slot-scope="text">
          <a-badge v-if="text" status="success" text="是"/>
          <a-badge v-else status="error" text="否"/>
        </span>
        <!-- <span slot="createTime" slot-scope="text">{{ text*1000 | moment }}</span> -->
        <span slot="createTime" slot-scope="text">{{ text | dayFormat('YYYY-MM-DD HH:mm:ss') }}</span>
        <span slot="action" slot-scope="text, record">
          <a @click="$refs.edit.show(record)" v-action:modify>编辑</a>
          <a-divider type="vertical" />
          <a-dropdown>
            <a class="ant-dropdown-link">更多
              <a-icon type="down" />
            </a>
            <a-menu slot="overlay">
              <a-menu-item v-action:run @click="handleRun(record.jobName)">
                立即运行
              </a-menu-item>
              <a-menu-item v-action:delete @click="handleDelete(record.jobName)">
                删除
              </a-menu-item>
            </a-menu>
          </a-dropdown>
        </span>
      </a-table>

      <!--功能模块-->
      <role-edit ref="edit" @complete="loadDataing"></role-edit>
      <role-add ref="add" @complete="loadDataing"></role-add>
    </a-card>
  </div>
</template>

<script>
import HeadInfo from '@/components/tools/HeadInfo'
import RoleEdit from './Edit'
import RoleAdd from './Add'
import { jobList, getJobMorniApi, runJobRightNow, deleteJobByName } from '@/api/control'
// import { getRoleList, modifyRoleStatus, deleteRole } from '@/api/control'
export default {
  name: 'TableList',
  components: {
    RoleEdit, RoleAdd, HeadInfo
  },
  data () {
    return {
      MorniApi: null,
      description:
        '',
      // 查询参数
      queryParam: {
        vague: '',
        pagedCount: 10,
        skipPage: 1
      },
      tables: {
        loading: false,
        dataSource: [],
        // 分页对象
        pagination: {
          showTotal: (total) => `总计 ${total} 条`,
          hideOnSinglePage: true,
          showSizeChanger: true,
          total: 0,
          defaultPageSize: 5,
          showQuickJumper: true
        },
        // 表头
        columns: [
          {
            title: '名称',
            dataIndex: 'jobName'
          },
          {
            title: '队列',
            dataIndex: 'queueName'
          },
          // {
          //   title: '状态',
          //   dataIndex: 'enable',
          //   scopedSlots: {
          //     customRender: 'enable'
          //   }
          // },
          {
            title: '地址',
            dataIndex: 'url',
            width: 10
          },
          {
            title: '重试',
            dataIndex: 'isRetry',
            scopedSlots: {
              customRender: 'enable'
            }
          },
          {
            title: '上次执行时间',
            dataIndex: 'lastExecution',
            width: 100,
            sorter: false,
            scopedSlots: {
              customRender: 'createTime'
            }
          },
          {
            title: '下次执行时间',
            dataIndex: 'nextExecution',
            width: 100,
            sorter: false,
            scopedSlots: {
              customRender: 'createTime'
            }
          },
          {
            title: '创建时间',
            dataIndex: 'createdAt',
            width: 100,
            sorter: false,
            scopedSlots: {
              customRender: 'createTime'
            }
          },
          {
            title: '操作',
            width: '150px',
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
    this.getmorniapi()
    // this.getRoleList()
  },
  methods: {
    getmorniapi () {
      getJobMorniApi().then(res => {
        console.log(res.result)
        this.MorniApi = res.result
      })
    },
    /**
    *搜索 查询列表
    */
    search () {
      this.queryParam.skipPage = 1
      this.loadDataing()
    },
    getRoleList () {
      jobList({ 'searchText': '', 'page': 1, 'pageSize': 999999 }).then(res => {
        console.log(res)
      })
    },
    /**
     * 加载数据方法
     * @param {Object} pagination 分页选项器
     * @param {Object} filters 过滤条件
     * @param {Object} sorter 排序条件
     */
    loadDataing (pagination, filters, sorter) {
      this.tables.loading = true
      if (pagination) {
        this.queryParam.skipPage = pagination.current
        this.queryParam.pagedCount = pagination.pageSize
      }
      jobList(this.queryParam).then(res => {
        this.tables.loading = false
        if (res.result.status === 200) {
          this.tables.pagination.total = res.result.totalCount
          this.tables.dataSource = res.result.result
          console.log(res.result.result)
        } else {
          this.$notification.error({ message: '加载失败', description: res.result.message })
        }
      }).catch(() => {
        this.tables.loading = false
      })
    },
    /**
     * 执行任务
     *@param {String} name 名称
     */
    handleRun (name) {
      const _this = this
      this.$confirm({ title: '执行任务',
        content: `是否立即执行 ${name} ？`,
        onOk: () => {
          runJobRightNow(name).then(res => {
            if (res.result.errorcode === '1') {
              this.loadDataing()
              this.getmorniapi()
              _this.$message.success(`执行任务 ${name} 成功`)
            } else {
              _this.$message.error('执行任务失败;' + res.result.message)
            }
          })
        }
      })
    },
    /**
     * 删除任务
     *@param {String} name 名称
     */
    handleDelete (name) {
      const _this = this
      this.$confirm({ title: '删除周期任务',
        content: `是否删除 ${name} ？`,
        onOk: () => {
          deleteJobByName(name).then(res => {
            if (res.result.errorcode === '1') {
              this.loadDataing()
              _this.$message.success(`删除任务 ${name} 成功`)
            } else {
              _this.$message.error('删除任务失败;' + res.result.message)
            }
          })
        }
      })
    }
  }
}
</script>
