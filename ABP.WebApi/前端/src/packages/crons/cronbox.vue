<template>
  <a-popover>
    <div class="iconbox" slot="content">
      <v-button
        v-for="item in data"
        :key="item.key"
        @click="handleClick(item)"
      >
        {{ item.key }}
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
import crondata from '@/api/cron'
export default {
  name: 'VCronbox',
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
      data: crondata
    }
  },
  computed: {
    currVal () {
      return this.value ? this.value : 'icon-supply'
    }
  },
  methods: {
    handleClick (val) {
      this.$emit('input', val.value)
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
