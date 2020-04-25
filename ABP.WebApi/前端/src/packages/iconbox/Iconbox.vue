<template>
  <a-popover>
    <span slot="title" v-if="!selectType">
      <span>
        <a-button
          size="small"
          :type="type==1?'primary':'default'"
          @click="type=1"
          style="margin-right:20px"
        >复制标签</a-button>
        <a-button size="small" :type="type==2?'primary':'default'" @click="type=2">复制属性</a-button></span>
    </span>
    <span slot="icon"></span>
    <span slot="cancelText"></span>
    <span slot="okText"></span>
    <div class="iconbox" slot="content">
      <v-button
        v-for="item in data"
        :key="item"
        @click="handleClick(item)"
        v-clipboard="{value:type==1?`<a-icon name='${item}' type='${item}' />`:item,success,error}"
      >
        <a-icon :name="item" :type="item"/>
      </v-button>
    </div>
    <slot>
      <a-input placeholder="default size" :value="currVal">
        <v-icon slot="addonBefore" :name="currVal" />
      </a-input>
    </slot>
  </a-popover>
</template>

<script>
const icons = [
  'lock', 'unlock', 'bars', 'book', 'calendar', 'cloud', 'cloud-download', 'code', 'copy', 'credit-card', 'delete', 'desktop', 'download', 'ellipsis', 'file', 'file-text', 'file-unknown', 'file-pdf', 'file-word', 'file-excel', 'file-jpg', 'file-ppt', 'file-markdown', 'file-add', 'folder', 'folder-open', 'folder-add', 'hdd', 'frown', 'meh', 'smile', 'inbox', 'laptop', 'appstore', 'link', 'mail', 'mobile', 'notification', 'paper-clip', 'picture', 'poweroff', 'reload', 'search', 'setting', 'share-alt', 'shopping-cart', 'tablet', 'tag', 'tags', 'to-top', 'upload', 'user', 'video-camera', 'home', 'loading', 'loading-3-quarters', 'cloud-upload', 'star', 'heart', 'environment', 'eye', 'camera', 'save', 'team', 'solution', 'phone', 'filter', 'exception', 'export', 'customer-service', 'qrcode', 'scan', 'like', 'dislike', 'message', 'pay-circle', 'calculator', 'pushpin', 'bulb', 'select', 'switcher', 'rocket', 'bell', 'disconnect', 'database', 'compass', 'barcode', 'hourglass', 'key', 'flag', 'layout', 'printer', 'sound', 'usb', 'skin', 'tool', 'sync', 'wifi', 'car', 'schedule', 'user-add', 'user-delete', 'usergroup-add', 'usergroup-delete', 'man', 'woman', 'shop', 'gift', 'idcard', 'medicine-box', 'red-envelope', 'coffee', 'copyright', 'trademark', 'safety', 'wallet', 'bank', 'trophy', 'contacts', 'global', 'shake', 'api', 'fork', 'dashboard', 'table', 'profile', 'alert', 'audit', 'branches', 'build', 'border', 'crown', 'experiment', 'fire', 'money-collect', 'property-safety', 'read', 'reconciliation', 'rest', 'security-scan', 'insurance', 'interation', 'safety-certificate', 'project', 'thunderbolt', 'block', 'cluster', 'deployment-unit', 'dollar', 'euro', 'pound', 'file-done', 'file-exclamation', 'file-protect',
  'file-search', 'file-sync', 'gateway', 'gold', 'robot', 'shopping'
]
export default {
  name: 'VIconbox',
  props: {
    value: {
      type: String,
      default: 'icon-supply'
    },
    selectType: {
      type: String,
      default: '',
      validator (val) {
        return ['tag', 'name', ''].indexOf(val) !== -1
      }
    }
  },
  data () {
    return {
      type: 1,
      data: Object.freeze(icons)
    }
  },
  computed: {
    currVal () {
      return this.value ? this.value : 'icon-supply'
    }
  },
  methods: {
    handleClick (val) {
      this.$emit('input', val)
    },
    success () {
      this.$message.info('已复制到剪切板')
    },
    error () {
      this.$message.info('复制失败')
    }
  }
}
</script>

<style lang="less" >
.iconbox {
  width: 400px;
}
</style>
