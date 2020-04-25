/* eslint-disable */
import api from './index'
import { axios } from '@/utils/request'
import Mgr from '@/utils/SecurityService'

/**
 * login func
 * parameter: {
 *     username: '',
 *     password: '',
 *     remember_me: true,
 *     captcha: '12345'
 * }
 * @param parameter
 * @returns {*}
 */
export function login(parameter) {
    return axios({
        url: 'asf/authorise/login',
        method: 'post',
        data: parameter,
        errorIntercept:false
    })
}

export function getSmsCaptcha(parameter) {
    return axios({
        url: api.SendSms,
        method: 'post',
        data: parameter
    })
}

export function getInfo() {
    return axios({
        url: '/account/info',
        //url: '/user/info',
        method: 'get',
        headers: {
            'Content-Type': 'application/json;charset=UTF-8'
        }
    })
}

export function logout() {
  const user = new Mgr()
  user.signOut()
}

/**
 * get user 2step code open?
 * @param parameter {*}
 */
export function get2step(parameter) {
    return axios({
        url: api.twoStepCode,
        method: 'post',
        data: parameter
    })
}