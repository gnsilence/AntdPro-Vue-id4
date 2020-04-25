<template>
  <div class="user-wrapper">
    <div class="content-box">
      <v-button tip="帮助" @click="handelhelp()" class="hidden-xs-only">
        <a-iconfont type="icon-bangzhu" />
      </v-button>
      <!-- <v-button tip="联系客服" @click="handelchat()" class="hidden-xs-only">
        <a-iconfont type="icon-icon_qq" />
      </v-button> -->
      <v-button :tip="screen?'退出全屏':'全屏'" class="hidden-xs-only" @click="toggleScreen">
        <a-iconfont :type="screen?'icon-fullscreen':'icon-expend'" />
      </v-button>
      <v-button tip="锁屏" @click="$router.push('/lock')" class="hidden-xs-only">
        <a-iconfont type="icon-jiesuo" />
      </v-button>
      <notice-icon class="action"/>
      <a-dropdown>
        <span class="action ant-dropdown-link user-dropdown-menu">
          <a-avatar class="avatar" size="small" :src="avatar()"/>
          <span>{{ nickname() }}</span>
        </span>
        <a-menu slot="overlay" class="user-dropdown-menu-wrapper">
          <a-menu-item key="0">
            <router-link :to="{ name: 'center' }">
              <a-icon type="user"/>
              <span>个人中心</span>
            </router-link>
          </a-menu-item>
          <a-menu-item key="2">
            <router-link :to="{ name: 'calendar' }">
              <a-icon type="calendar"/>
              <span>我的代办</span>
            </router-link>
          </a-menu-item>
          <a-menu-item key="1">
            <router-link :to="{ path: '' }">
              <a-icon type="setting"/>
              <span @click="handelset">账户设置</span>
            </router-link>
            </v-button>
          </a-menu-item>
          <a-menu-divider/>
          <a-menu-item key="3">
            <a href="javascript:;" @click="handleLogout">
              <a-icon type="logout"/>
              <span>退出登录</span>
            </a>
          </a-menu-item>
        </a-menu>
      </a-dropdown>
      <v-chat v-model="chartShow"></v-chat>
    </div>
  </div>
</template>

<script>
// import Mgr from '@/utils/SecurityService'
import NoticeIcon from '@/components/NoticeIcon'
import { mapActions, mapGetters } from 'vuex'
export default {
  name: 'UserMenu',
  components: {
    NoticeIcon
  },
  data () {
    return {
      // mgr: new Mgr(),
      screen: false,
      chartShow: false
    }
  },
  methods: {
    ...mapActions(['Logout']),
    ...mapGetters(['nickname', 'avatar']),
    handelhelp () {
      window.open('https://pro.loacg.com/docs/getting-started')
    },
    handelchat () {
      this.chartShow = true
    },
    handelset () {
      window.open(window.ApiUrl.StsServer + '/Manage')
    },
    toggleScreen () {
      if (!this.screen) {
        var docElm = document.documentElement
        if (docElm.requestFullscreen) {
          docElm.requestFullscreen()
        } else if (docElm.mozRequestFullScreen) {
          docElm.mozRequestFullScreen()
        } else if (docElm.webkitRequestFullScreen) {
          docElm.webkitRequestFullScreen()
        } else if (docElm.msRequestFullscreen) {
          docElm.msRequestFullscreen()
        } else {
          this.$message.error({
            content: '除了让你升级浏览器对方没什么好说的！',
            duration: 3
          })
        }
        this.screen = true
      } else {
        if (document.exitFullscreen) {
          document.exitFullscreen()
        } else if (document.mozCancelFullScreen) {
          document.mozCancelFullScreen()
        } else if (document.webkitCancelFullScreen) {
          document.webkitCancelFullScreen()
        } else if (document.msExitFullscreen) {
          document.msExitFullscreen()
        } else {
          this.$message.error({
            content: '请升级浏览器，不然我是不会理你的！',
            duration: 3
          })
        }
        this.screen = false
      }
    },
    handleLogout () {
      const that = this
      this.$confirm({
        title: '提示',
        content: '真的要注销登录吗 ?',
        onOk () {
          // return that.Logout({}).then(() => {
          //   window.location.reload()
          // }).catch(err => {
          //   that.$message.error({
          //     title: '错误',
          //     description: err.message
          //   })
          // })
          return that.Logout()
        },
        onCancel () {
        }
      })
    }
  }
}
</script>
