<template>
  <s-modal
    ref="modal"
    title="修改周期任务"
    :maskClosable="false"
    :destroyOnClose="true"
    :width="500"
    :confirmLoading="confirmLoading"
    :visible="visible"
    @ok="submit"
    @cancel="close">
    <a-spin :spinning="confirmLoading">
      <a-form :form="form" >
        <a-form-item label="任务名称" v-bind="layout">
          <a-input placeholder="请输入任务名称 " v-decorator="formDecorator.jobName" style="width: 50%"/>
        </a-form-item>
        <a-form-item label="Method" v-bind="layout">
          <a-select placeholder="请选择Http请求方法" :options="httpMethods" v-decorator="formDecorator.method">
          </a-select>
        </a-form-item>
        <a-form-item label="ContentType" v-bind="layout">
          <a-select placeholder="请选择ContentType" :options="ContentType" v-decorator="formDecorator.contentType">
          </a-select>
        </a-form-item>
        <a-form-item label="队列名称" v-bind="layout">
          <a-select placeholder="请选择队列名称" :options="QueueNames" v-decorator="formDecorator.queueName">
          </a-select>
        </a-form-item>
        <a-form-item label="接口地址" v-bind="layout">
          <a-input placeholder="接口地址,完整的请求路径" v-decorator="formDecorator.url" >
          </a-input></a-form-item>
        <a-form-item label="cron表达式" v-bind="layout">
          <v-cronbox v-model="corn">
            <a-input v-model="corn" placeholder="请输入cron表达式 " style="width: 40%"/>
          </v-cronbox>
        </a-form-item>
        <a-form-item label="是否重试" v-bind="layout">
          <a-switch checkedChildren="是" unCheckedChildren="否" v-decorator="formDecorator.isRetry"/>
        </a-form-item>
        <a-form-item label="Data" v-bind="layout">
          <a-input placeholder="Post请求时可以传入json数据" v-decorator="formDecorator.data" >
          </a-input></a-form-item>
        <a-form-item label="超时期限" v-bind="layout">
          <a-input placeholder="接口请求超时时间,单位秒" v-decorator="formDecorator.timeout" >
          </a-input></a-form-item>
      </a-form>
    </a-spin>
  </s-modal>
</template>

<script>
import { SModal } from '@/components'
import pick from 'lodash.pick'
import { addOrUpdareJob } from '@/api/control'
export default {
  name: 'PublicApiAdd',
  data () {
    return {
      visible: false,
      confirmLoading: false,
      corn: '* * * * *',
      form: this.$form.createForm(this),
      httpMethods: [
        { label: 'Post', value: 'POST' },
        { label: 'Get', value: 'GET' }
      ],
      ContentType: [
        { label: 'application/json', value: 'application/json' },
        { label: 'image/jpeg', value: 'image/jpeg' },
        { label: 'text/javascript', value: 'text/javascript' },
        { label: 'application/x-www-form-urlencoded', value: 'application/x-www-form-urlencoded' },
        { label: 'text/plain', value: 'text/plain' }
      ],
      QueueNames: [
        { label: 'apis', value: 'apis' },
        { label: 'apitest', value: 'apitest' },
        { label: 'oders', value: 'oders' },
        { label: 'plans', value: 'plans' },
        { label: 'rejobs', value: 'rejobs' },
        { label: 'percounts', value: 'percounts' },
        { label: 'default', value: 'default' }
      ],
      // 表单描述
      formDecorator: {
        jobName: ['jobName', {
          rules: [
            { required: true, message: '名称不能为空' },
            { max: 20 }
          ]
        }],
        isRetry: ['isRetry', { initialValue: true, valuePropName: 'checked' }],
        method: ['method', {
          rules: [
            { required: true, message: '请求方法不能为空' }
          ]
        }],
        contentType: ['contentType', {
          rules: [
            { required: true, message: '请求contentType不能为空' }
          ]
        }],
        queueName: ['queueName'],
        corn: ['corn'],
        data: ['data'],
        timeout: ['timeout'],
        url: ['url',
          {
            rules: [
              { required: true, message: 'API 地址不能为空' },
              { max: 500 }
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
      }
    }
  },
  components: {
    SModal
  },
  methods: {
    /**
     *显示添加对话框
     */
    show (data) {
      this.visible = true
      this.corn = data.corn
      this.$nextTick(() => {
        this.form.setFieldsValue(pick(data, 'jobName', 'isRetry', 'method', 'contentType', 'queueName', 'corn'
          , 'data', 'timeout', 'url'))
      })
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
        values.corn = this.corn
        this.confirmLoading = true
        addOrUpdareJob(values).then(res => {
          this.confirmLoading = false
          if (res.result.errorcode === '1') {
            this.$refs.modal.success(`编辑 ${values.jobName} 周期任务成功`)
            this.$emit('complete')
          } else {
            this.$refs.modal.error('编辑失败', res.result.message)
          }
        }).catch(() => { this.close() })
      })
    }
  }
}
</script>
