<template>
  <div>
    <v-card>
      <a-alert
        message="验证码组件"
        description="用于验证码及密码输入"
        type="info"
        showIcon>  </a-alert>
      <a-divider>掩码输入</a-divider>
      <a-button @click="visible=true">输入密码 {{ code }}</a-button>
      <VCaptcha v-model="visible" title="请输入密码" desc="默认密码为123456" :code.sync="code" mask></VCaptcha>
      <a-divider>明码输入</a-divider>
      <a-button @click="visible2=true">输入验证码 {{ code2 }}</a-button>
      <VCaptcha v-model="visible2" :code.sync="code2" desc="验证码已发送至手机159 **** 0460"></VCaptcha>
      <a-divider>自定义密码长度</a-divider>
      <a-button @click="visible3=true">输入验证码 {{ code3 }}</a-button>
      <VCaptcha v-model="visible3" maxlength="6" :code.sync="code3" ></VCaptcha>
    </v-card>
  </div>
</template>

<script>
export default {
  data () {
    return {
      visible: false,
      visible2: false,
      visible3: false,
      code: '',
      code2: '',
      code3: ''
    }
  }
}
</script>

<style>

</style>
