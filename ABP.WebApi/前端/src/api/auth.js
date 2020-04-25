import {
  post,
  get,
  Put,
  Delete
} from '@/utils/authrequest'

const api = {
  getAllRoles: '/Roles',
  getAllUsers: '/Users',
  addUserDto: '/Users',
  changePwd: '/Users/ChangePassword',
  addorUpdateRole: '/Users/Roles',
  updateUser: '/Users',
  getuserById: '/Users/',
  deleteUser: '/Users/',
  addRole: '/Roles',
  getUerRoles: '/Users/',
  deleteUserRole: '/Users/Roles'
}
export default api
// 获取所有角色
export function GetAllRoles (parmas) {
  return get(api.getAllRoles + '?searchText=' + parmas.searchText + '&page=' + parmas.page + '&pageSize=' + parmas.pageSize)
}
// 获取用户信息
export function GetAllUsers (parmas) {
  return get(api.getAllUsers + '?searchText=' + parmas.searchText + '&page=' + parmas.page + '&pageSize=' + parmas.pageSize)
}
// 根据id获取用户信息
export function GetUser (parmas) {
  return get(api.getuserById + parmas)
}
// 新增用户
export function addUser (parmas) {
  return post(api.addUserDto, parmas)
}
// 删除用户
export function deleteUser (parmas) {
  return Delete(api.deleteUser + parmas)
}
// 修改用户信息
export function UpdateUser (parmas) {
  return Put(api.updateUser, parmas)
}
// 修改密码
export function changePassword (parmas) {
  return post(api.changePwd, parmas)
}
// 添加或修改用户角色
export function AddOrUpdateUserRole (parmas) {
  return post(api.addorUpdateRole, parmas)
}
// 添加角色信息
export function AddRole (parmas) {
  return post(api.addRole, parmas)
}
// 获取用户角色信息
export function getUserRole (id, parmas) {
  return get(api.getUerRoles + id + '/roles?' + parmas)
}
// 删除用户角色
export function deleteUserRole (parmas) {
  return Delete(api.deleteUserRole, parmas)
}
