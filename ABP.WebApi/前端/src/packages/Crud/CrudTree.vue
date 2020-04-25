<template>
  <a-tree
    checkable
    multiple
    default-expand-all
    auto-expand-parent
    v-model="defaultVal"
    show-line
    :tree-data="treeData"
    @check="checkTree"
  />
</template>

<script>
export default {
  name: 'VCrudTree',
  props: {
    // eslint-disable-next-line vue/require-default-prop
    treeData: Array,
    // eslint-disable-next-line vue/require-default-prop
    field: String,
    // eslint-disable-next-line vue/require-default-prop
    initialValue: Array
  },
  data () {
    return {
      defaultVal: this.initialValue
    }
  },
  watch: {
    initialValue: {
      handler (val) {
        this.defaultVal = val
      }
    },
    deep: true
  },
  methods: {
    checkTree (checkedKeys, e) {
      this.$emit('check-tree', this.field, checkedKeys, e)
    }
  }
}
</script>

<style>
</style>
