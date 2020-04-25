// import { UserManager, WebStorageStateStore } from 'oidc-client'
// import store from '@/store'
// const STS_DOMAIN = 'http://localhost:5001/'
// const config = {
//   userStore: new WebStorageStateStore({ store: window.localStorage }),
//   authority: STS_DOMAIN,
//   client_id: 'vuejs_code_client',
//   redirect_uri: 'http://localhost:44357/callback.html',
//   automaticSilentRenew: true,
//   silent_redirect_uri: 'http://localhost:44357/silent-renew.html',
//   // accessTokenExpiringNotificationTime:4,
//   response_type: 'code',
//   scope: 'openid profile apitest email  roles Id4ClientId_api',
//   post_logout_redirect_uri: 'http://localhost:44357/',
//   filterProtocolClaims: true
// }
// var userManager = new UserManager(config)
// userManager.events.addUserLoaded(function (user) {
//   userManager.getUser().then(res => {
//     console.log('token refresh')
//     store.dispatch('SetToken', res.access_token).then(res => {})
//   })
// })
// userManager.events.addUserSignedOut(function () {
//   alert('Going out!');
//   console.log('UserSignedOut：', arguments);
//   userManager.signoutRedirect().then(function (resp) {
//     console.log('signed out', resp);
//   }).catch(function (err) {
//     console.log(err)
//   })
// });
// const AuthService = {
//   getUser () {
//     return userManager.getUser()
//   },
//   login () {
//     return userManager.signinRedirect()
//   },
//   logout () {
//     store.dispatch('RemoveToken').then(res => {})
//     return userManager.signoutRedirect()
//   },
//   getAccessToken () {
//     return userManager.getUser().then(res => {
//       return res.access_token
//     })
//   },
//   SetToken () {
//     return new Promise((resolve) => {
//       userManager.getUser().then(res => {
//         store.dispatch('SetToken', res.access_token)
//         resolve()
//       })
//     })
//   }
// }
// export default AuthService
