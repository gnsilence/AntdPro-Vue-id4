// ie polyfill
import '@babel/polyfill'

import Vue from 'vue'
import App from './App.vue'
import router from './core/router'
import store from './store/'
import { VueAxios } from './utils/request'
import VueTheMask from 'vue-the-mask'

import moment from 'moment'
import './common/directive'
import './packages'
import bootstrap from './core/bootstrap'
import './core/use'
import './permission' // permission control
import './utils/filter' // global filter
import * as filters from './common/filter'

Vue.config.productionTip = false
Vue.prototype.$moment = moment
// mount axios Vue.$http and this.$http
Vue.use(VueAxios)
Vue.use(VueTheMask)
Object.keys(filters).forEach(key => {
  Vue.filter(key, filters[key])
})
new Vue({
  router,
  store,
  created () {
    bootstrap()
  },
  render: h => h(App)
}).$mount('#app')
