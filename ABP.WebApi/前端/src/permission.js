import Vue from 'vue'
import router from './core/router'
import store from './store'

import NProgress from 'nprogress' // progress bar
import 'nprogress/nprogress.css' // progress bar style
import notification from 'ant-design-vue/es/notification'
import { setDocumentTitle, domTitle } from '@/utils/domUtil'
import { ACCESS_TOKEN } from '@/store/mutation-types'
import Mgr from '@/utils/SecurityService'
var user = new Mgr()

NProgress.configure({ showSpinner: false }) // NProgress Configuration

const whiteList = ['login', 'register', 'registerResult'] // no redirect whitelist

router.beforeEach((to, from, next) => {
  NProgress.start() // start progress bar
  to.meta && (typeof to.meta.title !== 'undefined' && setDocumentTitle(`${to.meta.title} - ${domTitle}`))
  if (Vue.ls.get(ACCESS_TOKEN)) {
    /* has token */
    if (to.path === '/user/login') {
      next({ path: '/' })
      NProgress.done()
    } else {
      if (store.getters.menus.length === 0) {
        store
          .dispatch('GetInfo')
          .then(res => {
            // console.log(res.result)
            if (res.result.menus.length === 0) {
              notification.error({ message: '错误', description: '用户角色禁用或没有角色权限' })
              setTimeout(function () {
                user.signOut()
              }, 4000)
              return false
            }
            const menus = res.result && res.result.menus
            store.dispatch('GenerateRoutes', menus).then(() => {
              // 根据roles权限生成可访问的路由表
              // 动态添加可访问路由表
              router.addRoutes(store.getters.dynamicRouters)

              const redirect = decodeURIComponent(from.query.redirect || to.path)

              if (to.path === redirect) {
                // hack方法 确保addRoutes已完成 ,set the replace: true so the navigation will not leave a history record
                next({ ...to, replace: true })
              } else {
                // 跳转到目的路由
                next({ path: redirect })
              }
            })
          })
          .catch(() => {
            notification.error({ message: '错误', description: '用户登陆失败或没有访问权限，请重试' })
            user.signOut()
            // store.dispatch('Logout').then(() => {
            //   next({ path: '/user/login', query: { redirect: to.fullPath } })
            // })
          })
      } else {
        next()
      }
    }
  } else {
    if (whiteList.includes(to.name)) {
      // 在免登录白名单，直接进入
      next()
    } else {
      // console.log(to)
      next({ path: '/user/login', query: { redirect: to.fullPath } })
      NProgress.done() // if current page is login will not trigger afterEach hook, so manually handle it
    }
  }
})

router.afterEach(() => {
  NProgress.done() // finish progress bar
})
