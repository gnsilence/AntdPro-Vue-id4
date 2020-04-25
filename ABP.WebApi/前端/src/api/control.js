import {
  post,
  get
} from '@/utils/request'

const api = {
  // 管理员
  createAccount: '/account/create',
  modifyAccount: '/account/midify',
  modifyStatusAccount: '/account/midifystatus',
  getAccountDetail: '/account/GetDetails',
  getAccountList: '/account/getlist',
  resetPassword: '/account/ResetPassword',
  deleteAccount: '/account/delete',

  // 菜单
  getMenuSimpleAll: '/menu/GetAllList',
  getMenuDetails: '/menu/GetDetails',
  getMenuList: '/menu/GetList',
  createMenu: '/menu/Create',
  modifyMenuSort: '/menu/ModifySort',
  modifyMenu: '/menu/Modify',
  deleteMenu: '/menu/Delete',
  exportMenu: '/menu/Export',
  importMenu: '/menu/Import',

  // 功能
  getActionList: '/action/GetList',
  getActionDetails: '/action/GetDetails',
  createAction: '/action/Create',
  modifyAction: '/action/Modify',
  modifyActionSort: '/action/ModifySort',
  deleteAction: '/action/Delete',

  // 角色
  getRoleSimpleList: '/role/GetListAll',
  getRoleDetail: '/role/GetDetails',
  createRole: '/role/Create',
  modifyRole: '/role/Modify',
  deleteRole: '/role/delete',
  modifyRoleStatus: '/role/ModifyStatus',
  getRoleList: '/role/getlist',
  existsRole: '/role/IfExistsRole',

  // 公共API
  getPublicApiList: '/openapi/GetList',
  createPublicApi: '/openapi/Create',
  modifyPublicApi: '/openapi/Modify',
  deletePublicApi: '/openapi/Delete',
  importPublicApi: '/openapi/Import',

  // 审计
  getAuditList: '/Audit/GetAuditLogsPagelist',
  deleteAudit: '/Audit/DeleteLogs',

  // 后台任务
  getjobList: '/BackJob/GetJobItems',
  addorupdatejob: '/BackJob/AddOrUpdateRecurringJob',
  getjobmorapi: '/BackJob/GetMonitoringApi',
  getjobbyname: '/BackJob/GetHttpJobItem',
  runjobrightnow: '/BackJob/TriggerRecurringJob',
  deletejobbyname: '/BackJob/DeleteJob'
}
export default api

// 操作功能
export function getActionList (parameter) {
  return post(api.getActionList, parameter, {
    errorRedirect: true
  })
}
export function getActionDetails (parameter) {
  return get(api.getActionDetails + '?id=' + parameter)
}
export function createAction (parameter) {
  return post(api.createAction, parameter)
}
export function modifyAction (parameter) {
  return post(api.modifyAction, parameter)
}
export function modifyActionSort (parameter) {
  return post(api.modifyActionSort, parameter)
}
export function deleteMenu (parameter) {
  return post(api.deleteMenu + '?id=' + parameter)
}

// 菜单
export function getMenuList (parameter) {
  return post(api.getMenuList, parameter, {
    errorRedirect: true
  })
}
export function getMenuDetails (parameter) {
  return get(api.getMenuDetails + '?id=' + parameter)
}
export function getMenuSimpleAll () {
  return get(api.getMenuSimpleAll)
}
export function modifyMenuSort (parameter) {
  return post(api.modifyMenuSort, parameter)
}
export function createMenu (parameter) {
  return post(api.createMenu, parameter)
}
export function modifyMenu (parameter) {
  return post(api.modifyMenu, parameter)
}
export function deleteAction (parameter) {
  return post(api.deleteAction + '?id=' + parameter)
}

export function exportMenu (parameter) {
  return post(api.exportMenu, parameter)
}

export function importMenu (parameter) {
  return post(api.importMenu, parameter)
}

// 角色
export function getRoleSimpleList () {
  return get(api.getRoleSimpleList)
}
export function ifExistsRole (parameter) {
  return get(api.existsRole + '?name=' + parameter)
}
export function getRoleDetail (parameter) {
  return get(api.getRoleDetail + '?id=' + parameter)
}
export function createRole (parameter) {
  return post(api.createRole, parameter)
}
export function modifyRole (parameter) {
  return post(api.modifyRole, parameter)
}
export function getRoleList (parameter) {
  return post(api.getRoleList, parameter, {
    errorRedirect: true
  })
}
export function modifyRoleStatus (parameter) {
  return post(api.modifyRoleStatus, parameter)
}
export function deleteRole (parameter) {
  return post(api.deleteRole + '?id=' + parameter)
}

// 管理员
export function createAccount (parameter) {
  return post(api.createAccount, parameter)
}
export function modifyAccount (parameter) {
  return post(api.modifyAccount, parameter)
}
export function resetPassword (parameter) {
  return post(api.resetPassword, parameter)
}
export function getAccountDetail (parameter) {
  return get(api.getAccountDetail + '?id=' + parameter)
}
export function getAccountList (parameter) {
  return post(api.getAccountList, parameter, {
    errorRedirect: true
  })
}
export function modifyStatusAccount (parameter) {
  return post(api.modifyStatusAccount, parameter)
}
export function deleteAccount (parameter) {
  return post(api.deleteAccount + '?id=' + parameter)
}

// 公共API
export function getPublicApiList (parameter) {
  return post(api.getPublicApiList, parameter, {
    errorRedirect: true
  })
}
export function createPublicApi (parameter) {
  return post(api.createPublicApi, parameter)
}
export function deletePublicApi (parameter) {
  return post(api.deletePublicApi + `?id=${parameter}`)
}
export function modifyPublicApi (parameter) {
  return post(api.modifyPublicApi, parameter)
}

export function importPublicApi (parameter) {
  return post(api.importPublicApi, parameter)
}

// 审计
export function getAuditList (parameter) {
  return post(api.getAuditList, parameter, {
    errorRedirect: true
  })
}
export function deleteAudit (parameter) {
  return post(api.deleteAudit, parameter)
}
// 后台任务
export function jobList (parameter) {
  return post(api.getjobList, parameter)
}
export function addOrUpdareJob (parameter) {
  return post(api.addorupdatejob, parameter)
}
export function getJobMorniApi () {
  return get(api.getjobmorapi)
}
export function getJobByName (parameter) {
  return get(api.getjobbyname + '?jobname=' + parameter)
}
export function runJobRightNow (parameter) {
  return get(api.runjobrightnow + '?jobname=' + parameter)
}
export function deleteJobByName (parameter) {
  return get(api.deletejobbyname + '?jobname=' + parameter)
}
