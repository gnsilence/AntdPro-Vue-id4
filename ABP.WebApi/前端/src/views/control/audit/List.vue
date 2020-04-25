<template>
  <a-card :bordered="false">

    <!--列表搜索-->
    <div class="table-page-search-wrapper">
      <a-form layout="inline">
        <a-row type="flex" justify="space-around" :gutter="48">
          <!-- <a-col :md="6" :sm="24">
            <a-form-item label="权限ID">
              <a-input placeholder="请输入权限ID" v-model="queryParam.permissionId" />
            </a-form-item>
          </a-col> -->
          <a-col :md="8" :sm="24">
            <a-form-item label="记录时间">
              <a-range-picker name="buildTime" @change="ChangeBenginEndTime" />
            </a-form-item>
          </a-col>
          <a-col :md="8" :sm="24">
            <a-form-item label="操作账号">
              <a-input placeholder="请输入操作账号" v-model="queryParam.query.Account" />
            </a-form-item>
          </a-col>
          <a-col :md="8" :sm="24">
            <div class="table-page-search-submitButtons" >
              <a-input-search
                placeholder="请输方法名称'"
                v-model="queryParam.query.MethodName"
                enterButton="查询"
                @search="Search" >
              </a-input-search>
            </div>
          </a-col></a-row>
        <a-row v-if="advanced" justify="end" :gutter="48">
          <a-col :md="8" :sm="24">
            <a-form-item label="客户端IP">
              <a-input placeholder="请输入客户端IP" v-model="queryParam.query.IPAddress" />
            </a-form-item>
          </a-col>
        </a-row>
      </a-form>
    </div>

    <!--列表操作-->
    <div class="table-operator" >
      <a-tooltip>
        <template slot="title">批量删除
        </template>
        <a-button v-action:delete type="primary" icon="delete" class="right10" @click="$refs.delete.show()"></a-button>
      </a-tooltip>
      <a-radio-group :defaultValue="2" buttonStyle="solid" class="right10" v-model="searchType" @change="Search">
        <a-radio-button :value="2">操作审计</a-radio-button>
        <a-radio-button :value="1">只看异常</a-radio-button>
      </a-radio-group>

      <a-tooltip>
        <template slot="title">高级查询
        </template>
        <a-button type="primary" @click="advanced= !advanced" style="float:right;margin-right:0;margin-left:10;" >
          <a-icon :type="advanced ? 'up' : 'down'" /></a-button>
      </a-tooltip>
    </div>

    <!--列表-->
    <a-table
      ref="table"
      rowKey="id"
      size="middle"
      :pagination="table.pagination"
      :columns="table.columns"
      :loading="table.loading"
      :dataSource="table.dataSource"
      @change="LoadDataing">
      <span slot="serviceName" slot-scope="text">
        {{ text }}
      </span>
      <!-- <span slot="permissionId" slot-scope="text">{{ (text===null?'--':text) }}</span> -->
      <span slot="methodName" slot-scope="text">{{ text }}
      </span>
      <!-- <span slot="parameters" slot-scope="text">{{ text }}</span> -->
      <!-- <span slot="exception" slot-scope="text">{{ text }}</span> -->
      <span slot="clientIpAddress" slot-scope="text">{{ text | replaceA(/::ffff:/, "") }}</span>
      <span slot="executionTime" slot-scope="text">{{ text | dayFormat('YYYY-MM-DD HH:mm:ss') }}</span>
      <span slot="action" slot-scope="text, record">
        <a @click="$refs.detail.show(record)" v-action:query>详情</a>
      </span>
    </a-table>

    <!--批量删除-->
    <audit-delete ref="delete" @complete="LoadDataing"></audit-delete>
    <!--详情-->
    <audit-detail ref="detail"></audit-detail>
  </a-card>
</template>

<script>
import { STable } from '@/components'
import { getAuditList } from '@/api/control'
import AuditDetail from './Detail'
import AuditDelete from './Delete'

export default {
  name: 'AuditList',
  components: {
    AuditDetail,
    AuditDelete,
    STable
  },
  data () {
    return {
      // 高级搜索 展开/关闭
      advanced: false,
      // 查询参数
      // queryParam: {
      //   subject: '',
      //   account: '',
      //   type: 2,
      //   beginTime: null,
      //   endTime: null,
      //   permissionId: '',
      //   clientIp: '',
      //   pagedCount: 10,
      //   skipPage: 1
      // },
      searchType: 2,
      queryParam: {
        query: {
          beginTime: '',
          endTime: '',
          hasException: false,
          Account: '',
          IPAddress: '',
          MethodName: ''
        },
        sorting: 'Id,desc',
        startPage: 1,
        pageCount: 10
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
            title: '编号',
            dataIndex: 'id',
            width: 80
          },
          {
            title: '服务名称',
            dataIndex: 'serviceName',
            width: 100
          },
          {
            title: '方法名称',
            dataIndex: 'methodName',
            width: 100,
            scopedSlots: {
              customRender: 'methodName'
            }
          },
          {
            title: '客户端IP',
            align: 'center',
            dataIndex: 'clientIpAddress',
            width: 100
          },
          {
            title: '记录时间',
            dataIndex: 'executionTime',
            width: 150,
            scopedSlots: {
              customRender: 'executionTime'
            }
          },

          // {
          //   title: '异常',
          //   dataIndex: 'exception',
          //   width: 150
          // },
          // {
          //   title: '参数',
          //   dataIndex: 'parameters',
          //   align: 'center',
          //   width: 180
          // },
          {
            title: '操作',
            align: 'center',
            width: 70,
            dataIndex: 'action',
            scopedSlots: {
              customRender: 'action'
            }
          }
        ]
      }
    }
  },
  filters: {
    replaceA (text, substr, replacement) {
      return text.replace(substr, replacement)
    },
    loggerType (status) {
      const statusMap = {
        1: '登录日志',
        2: '操作审计'
      }
      return statusMap[status === 1 ? 1 : 2]
    }
  },
  watch: {
    'queryParam.startPage' () {
      this.table.pagination.current = this.queryParam.startPage
    }
  },
  methods: {
    /**
     * 条件搜索事件
     */
    Search () {
      this.queryParam.startPage = 1
      if (this.searchType === 1) {
        this.queryParam.query.hasException = true
      } else {
        this.queryParam.query.hasException = false
      }
      this.LoadDataing()
    },
    /**
     * 加载数据方法
     * @param {Object} pagination 分页选项器
     * @param {Object} filters 过滤条件
     * @param {Object} sorter 排序条件
     */
    LoadDataing (pagination, filters, sorter) {
      this.table.loading = true
      if (pagination) {
        this.queryParam.startPage = pagination.current
        this.queryParam.pageCount = pagination.pageSize
      }
      getAuditList(this.queryParam).then(res => {
        this.table.loading = false
        if (res.success === true) {
          this.table.pagination.total = res.result.totalCount
          this.table.dataSource = res.result.items
        } else {
          this.$notification.error({ message: '加载失败', description: res.result.message })
        }
      }).catch(() => {
        this.table.loading = false
      })
    },
    // 查询条件开始和结束时间录入
    ChangeBenginEndTime (date, dateStrings) {
      this.queryParam.query.beginTime = dateStrings[0]
      this.queryParam.query.endTime = dateStrings[1]
    }
  },
  created () {
    this.LoadDataing()
  }

}
</script>

<style lang="less" scoped>
.search {
  margin-bottom: 54px;
}
.fold {
  width: calc(100% - 216px);
  display: inline-block;
}
.right10 {
   margin-right:10px;
}
.operator {
  margin-bottom: 18px;
}
@media screen and (max-width: 900px) {
  .fold { width: 100%;}
}
</style>
