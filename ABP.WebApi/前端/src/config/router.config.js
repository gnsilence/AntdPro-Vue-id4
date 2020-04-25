// eslint-disable-next-line
import { UserLayout, BasicLayout, RouteView, BlankLayout, PageView } from '@/layouts'
import { RoleList, AuditList, AdminList, PublicApiList, MenuList, MenuDetails, JobList } from '@/views/control'
/**
 * 动态路由
 * @type { Array }
 */
export const asyncRouterMap = [{
  path: '/',
  name: 'index',
  component: BasicLayout,
  meta: { title: '首页' },
  redirect: '/profiles/profiles',
  children: [
    // dashboard
    {
      path: '/dashboard',
      name: 'dashboard',
      redirect: '/dashboard/analysis',
      component: RouteView,
      children: [{
        path: '/dashboard/analysis',
        name: 'dashboard_analysis',
        component: () => import('@/views/dashboard/Analysis')
      },
      {
        path: '/dashboard/workplace',
        name: 'dashboard_workplace',
        component: () => import('@/views/dashboard/Workplace')
      }]
    },
    // control
    {
      path: '/control',
      name: 'asf',
      redirect: '/control/admin',
      component: PageView,
      children: [
        // 管理员列表
        {
          path: '/control/admin',
          name: 'asf_account',
          component: AdminList
        },
        // 角色列表
        {
          path: '/control/role',
          name: 'asf_role',
          component: RoleList
        },
        // 任务管理
        {
          path: '/control/job',
          name: 'asf_job',
          component: JobList
        },
        // 菜单
        {
          path: '/control/menu',
          name: 'asf_menu',
          component: RouteView,
          redirect: '/control/menu/list',
          children: [
            {
              path: '/control/menu/details',
              name: 'asf_menu_details',
              component: MenuDetails
            },
            {
              path: '/control/menu/list',
              name: 'asf_menu_list',
              component: MenuList
            }
          ]
        },
        // 审计管理
        {
          path: '/control/audit',
          name: 'asf_audit',
          component: AuditList
        },
        // 公共API管理
        {
          path: '/control/publicapi',
          name: 'asf_publicapi',
          component: PublicApiList
        }
      ]
    }, {
      path: '/widgets',
      name: 'widgets',
      component: RouteView,
      children: [
        /* 城市选择器 */
        {
          path: '/widgets/city-picker',
          name: 'widgets_city-picker',
          component: () => import('@/views/widgets/city-picker')
        },
        /* 金额过滤 */
        {
          path: '/widgets/amount',
          name: 'widgets_amount',
          component: () => import('@/views/widgets/amount')
        },
        /* 车牌选择器 */
        {
          path: '/widgets/licence-plate',
          name: 'widgets_licence-plate',
          component: () => import('@/views/widgets/licence-plate')
        },
        /* 图标选择器 */
        {
          path: '/widgets/iconbox',
          name: 'widgets_iconbox',
          component: () => import('@/views/widgets/iconbox')
        },
        /* 向导 */
        {
          path: '/widgets/guide',
          name: 'widgets_guide',
          component: () => import('@/views/widgets/guide')
        },
        /* 筛选数据 */
        {
          path: '/widgets/filterbox',
          name: 'widgets_filterbox',
          component: () => import('@/views/widgets/filterbox')
        },
        /* 验证码 */
        {
          path: '/widgets/captcha',
          name: 'widgets_captcha',
          component: () => import('@/views/widgets/captcha')
        },
        /* 格式化 */
        {
          path: '/widgets/mask',
          name: 'widgets_mask',
          component: () => import('@/views/widgets/mask')
        },
        /* 头像 */
        {
          path: '/widgets/avatar-group',
          name: 'widgets_avatar-group',
          component: () => import('@/views/widgets/avatar-group')
        },
        /* 打印 */
        {
          path: '/widgets/print',
          name: 'widgets_print',
          component: () => import('@/views/widgets/print')
        }
      ]
    }
  ]
},
{
  path: '*',
  redirect: '/404'
}]

/**
 * 基础路由
 * @type { Array }
 */
export const constantRouterMap = [
  {
    path: '/user',
    component: UserLayout,
    redirect: '/user/login',
    hidden: true,
    children: [
      {
        path: 'login',
        name: 'login',
        component: resolve => require(['@/views/user/Login'], resolve)
      }]
  },
  {
    path: '/lock',
    component: () => import('@/components/tools/lock'),
    name: 'lock'
  },
  // {
  //   path: '/calendar',
  //   component: () => import('@/views/user/calendar'),
  //   name: 'calendar'
  // },
  // 异常页面
  {
    path: '/exception',
    component: BasicLayout,
    children: [{
      path: '/404',
      name: '404',
      component: () => import('@/views/exception/404'),
      meta: { title: '404' }
    }, {
      path: '/403',
      name: '403',
      component: () => import('@/views/exception/403'),
      meta: { title: '403' }
    },
    {
      path: '/500',
      name: '500',
      component: () => import('@/views/exception/500'),
      meta: { title: '500' }
    }]
  },
  // 个人页面
  {
    path: '/profiles',
    component: BasicLayout,
    hidden: true,
    name: 'profiles',
    meta: { title: '个人页', icon: 'user' },
    children: [{
      path: '/profiles/profiles',
      name: 'center',
      component: () => import('@/views/profiles/center/Index'),
      meta: { title: '个人中心' }
    },
    {
      path: '/profiles/calendar',
      name: 'calendar',
      component: () => import('@/views/user/calendar'),
      meta: { title: '代办' }
    },
    {
      path: '/profiles/settings',
      name: 'settings',
      component: () => import('@/views/profiles/settings/Index'),
      redirect: '/profiles/settings/Security',
      children: [{
        path: '/profiles/settings/Security',
        name: 'SecuritySettings',
        component: () => import('@/views/profiles/settings/Security'),
        meta: { title: '账户设置' }
      },
      {
        path: '/profiles/settings/custom',
        name: 'CustomSettings',
        component: () => import('@/views/profiles/settings/Custom'),
        meta: { title: '个性化设置' }
      },
      {
        path: '/profiles/settings/notification',
        name: 'NotificationSettings',
        component: () => import('@/views/profiles/settings/Notification'),
        meta: { title: '新消息通知' }
      }]
    }]
  },
  {
    path: '/test',
    component: BlankLayout,
    redirect: '/test/home',
    children: [
      {
        path: 'home',
        name: 'TestHome',
        component: () => import('@/views/Home')
      }
    ]
  }]
