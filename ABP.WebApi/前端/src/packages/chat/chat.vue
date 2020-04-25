<template>
  <!-- v-draggable="draggableValue" -->
  <div class="artiely-chat" v-if="value" v-drag="{trigger:'.chat-top',body:'.artiely-chat'}">
    <!-- 头部可拖拽 -->
    <div class="chat-top">
      <div class="top-l">
        <v-avatar-group :avatarList="list1" :maxlength="3" :size="20" style="margin-top:10px"></v-avatar-group>
        <span>排队中...</span>
      </div>
      <div class="top-m">谭杰</div>
      <div class="top-r">
        快捷回复
        <!-- <v-icon name="icon-fullscreen" @click.native="close"></v-icon> -->
        <v-button tip="最小化" @click.native="close">
          <a-iconfont type="icon-fullscreen" />
        </v-button>
      </div>
    </div>
    <!-- 主容器 -->
    <div class="chat-container">
      <!-- 客户列表 -->
      <div class="chat-custom-wrapper">
        <div class="custom-wrapper">
          <div class="custom-list" v-for="item in list2" :key="item.id">
            <a-avatar
              slot="avatar"
              :style="{background:item.color}"
            >{{ item.name.split('')[0] }}</a-avatar>
            <div class="userinfo">
              <h1 class="name">{{ item.name }}</h1>
              <h1 class="tel number">{{ item.tel|telFormat() }}</h1>
            </div>
            <a-badge :count="item.count" class="message-count" />
          </div>
        </div>
      </div>
      <!-- 聊天面板 -->
      <div class="chat-content-wrapper">
        <div class="chat-list">
          <div class="chat-body">
            <div class="left">
              <div class="avatar">
                <a-avatar src="https://zos.alipayobjects.com/rmsportal/ODTLcjxAfvqbxHnVXCYX.png" />
              </div>
              <div class="message-box">
                <div class="message">你们这是什么服务态度啊！！！你们这是什么服务态度啊！！！你们这是什么服务态度啊！！！你们这是什么服务态度啊！！！</div>
              </div>
            </div>
            <div class="left">
              <div class="avatar">
                <a-avatar src="https://zos.alipayobjects.com/rmsportal/ODTLcjxAfvqbxHnVXCYX.png" />
              </div>
              <div class="message-box">
                <div class="message">你们这是什么服务态度啊！！！</div>
              </div>
            </div>
            <div class="right">
              <div class="message-box">
                <div class="message">亲~，您听我解释...</div>
              </div>
              <div class="avatar">
                <a-avatar src="https://zos.alipayobjects.com/rmsportal/ODTLcjxAfvqbxHnVXCYX.png" />
              </div>
            </div>
          </div>
        </div>
        <div class="chat-input">
          <div class="input-action">
            <a-iconfont type="icon-fullscreen" />
            <VEmojiPicker @emoji="handleEmoji"><a-iconfont type="icon-fullscreen"/></VEmojiPicker>
          </div>
          <!-- 输入 -->
          <a-textarea
            placeholder="客户就是衣食父母~！"
            class="input-field"
            :autosize="{ minRows: 2, maxRows: 2 }"
            v-model="messageInfo"
          />
          <div class="send">
            <v-icon name="icon-fasong"></v-icon>
          </div>
        </div>
      </div>
      <!-- 快速回复 -->
      <div class="message-list">
        <ul>
          <li class="message-item" v-for="i in list" :key="i">{{ i }}</li>
        </ul>
      </div>
    </div>
  </div>
</template>

<script>
const list = [
  'Hello，周末愉快！很高兴为你效劳',
  '您好，欢迎光临本小店，请问您观看中哪些宝贝？我可以帮你介绍一下',
  '您好，欢迎光临足康乐旗舰店，很高兴为您服务,请问有什么可以帮助您！',
  '您好！请问有什么可以为您效劳吗？',
  '您好！欢迎光临足康乐旗舰店，请问可以帮助到您吗？',
  '您好！欢迎光临足康乐旗舰店，很高兴为您效劳！',
  '好吧，如果您相信我的个人意见，我给您推荐几款，存属个人意见哈，呵呵……',
  '我们的价格是最优惠的了',
  '你还有什么不了解或者不明白的地方吗？ ',
  '忘了告诉你，我这几天正好在促销，优惠很大的'
]
export default {
  name: 'VChat',
  props: {
    value: Boolean
  },
  data () {
    return {
      list,
      messageInfo: '',
      // list2: [],
      'list2': [{
        'name': '刘涛',
        'tel': '17459085784',
        'age': 15,
        'count': 8,
        'birthday': '1996-03-16',
        'datetime': '1972-11-01 20:36:42',
        'now': '2020-01-20 15:20:41',
        'email': 'i.xcixmpkky@vjyspoflr.tk',
        'city': '汕头市',
        'color': '#f279bf',
        'id': '140000200309030556',
        'image': 'http://dummyimage.com/200x200',
        'tag': 'Sjbbvmbpje Lirht Mbuvpfdjq Uckagdhk',
        'remark': '四干支或影说东效连提片至更。任叫由着了又日经风场听定向器除土族题。条结运复相备其林被两性着金工。点属些此民即其度除压看处。质或员设队听点新才平深对进验给管各备。第大等品别价及上式满带亲山飞具性美。',
        'desc': '走被直半开切运厂素己的建来整看专器度。'
      }, {
        'name': '廖刚',
        'tel': '15769585668',
        'age': 12,
        'count': 8,
        'birthday': '1970-08-07',
        'datetime': '2017-11-29 02:35:40',
        'now': '2020-01-20 15:20:41',
        'email': 't.xyuqiubtxe@rcmgg.es',
        'city': '重庆市',
        'color': '#79e3f2',
        'id': '210000198104222595',
        'image': 'http://dummyimage.com/200x200',
        'tag': 'Soxdedrum Xigagoww Hedjt Hwnvd',
        'remark': '任约月产空题严千西实文除流放三农。众斗才车商光西国收速民速白专龙必教。的地八统民原须长入准名种究只科对全。',
        'desc': '百直织位克此路反一加特风着。'
      }, {
        'name': '杜涛',
        'tel': '11428835618',
        'age': 75,
        'count': 6,
        'birthday': '2016-05-04',
        'datetime': '1988-11-07 16:43:47',
        'now': '2020-01-20 15:20:41',
        'email': 'b.qientztd@nvuphbndqs.ng',
        'city': '汕尾市',
        'color': '#f2dd79',
        'id': '450000199812142333',
        'image': 'http://dummyimage.com/200x200',
        'tag': 'Upqqbyy Heqk Eqlr',
        'remark': '广消心还领天民术而置何据统收品断。到程代级美教情目中准信真除这。酸里不教化易习证派达资照路她名。通名那团与什通话下团原商照参究文。下商边高约会点量流按开体院习原通。',
        'desc': '运千体收所作参活半化构认铁议。'
      }, {
        'name': '魏勇',
        'tel': '19337455425',
        'age': 89,
        'count': 2,
        'birthday': '1971-08-31',
        'datetime': '2014-11-07 10:19:33',
        'now': '2020-01-20 15:20:41',
        'email': 'v.gulywxx@yearfrpsif.name',
        'city': '柳州市',
        'color': '#ba79f2',
        'id': '820000197803241380',
        'image': 'http://dummyimage.com/200x200',
        'tag': 'Lhcb Bhbcxjt Vcykc Bopjjnvxp Bdghknnuqd Lxvvez Sgoblujmi',
        'remark': '活从技除厂思极开众美几改集。出马样属果团号火组养产眼调称。县去走段该主二共非问不称车十它常。应体可军开义而口象心我位。号研真队切及近题系例适县了到界层节。府厂北步任想参还调连改要后。达该人京经克党应件力务听者。',
        'desc': '空力得头正上九断入美报也商全商处政花。'
      }, {
        'name': '沈杰',
        'tel': '13916162643',
        'age': 23,
        'count': 2,
        'birthday': '1985-04-18',
        'datetime': '2006-08-12 18:06:51',
        'now': '2020-01-20 15:20:41',
        'email': 'h.yxqfxpkex@btge.is',
        'city': '澳门半岛',
        'color': '#79f297',
        'id': '54000020080917434X',
        'image': 'http://dummyimage.com/200x200',
        'tag': 'Reqvb Polpiuus Doydlmqp Lukuciwd Jyjedg',
        'remark': '对月才住每便他历半院才拉它天由。得路半要工片效高际听山相候体求。局的了周亲计光书直人那深。向志日济便最通加动算机研联十米利。酸儿统会心历亲花京决将容构结合而志。',
        'desc': '商提系代经例器问将压各已运团。'
      }, {
        'name': '黎平',
        'tel': '11131229584',
        'age': 70,
        'count': 6,
        'birthday': '2014-11-17',
        'datetime': '1982-11-16 15:21:42',
        'now': '2020-01-20 15:20:41',
        'email': 'r.gvpbhollfl@ddjx.gy',
        'city': '辽阳市',
        'color': '#f2797e',
        'id': '820000199906010760',
        'image': 'http://dummyimage.com/200x200',
        'tag': 'Qbly Xuxd Zosxrfc Zwihe Flcfpg',
        'remark': '水将建照还收决走由平动反大。段论身研去相林想品常正府干阶前。意低标组速规空示按难主非业但全有条。主压断称备决群复使走存矿至门。',
        'desc': '天料过指程往标属称将色工。'
      }, {
        'name': '许秀兰',
        'tel': '11744923337',
        'age': 13,
        'count': 9,
        'birthday': '1983-03-02',
        'datetime': '2019-09-20 22:45:05',
        'now': '2020-01-20 15:20:41',
        'email': 't.kplq@flhjxhld.il',
        'city': '九龙',
        'color': '#79a1f2',
        'id': '340000198905084988',
        'image': 'http://dummyimage.com/200x200',
        'tag': 'Qzxadptvm Wbrv Bjtwkmfb Vrnfu Yiet Cswpflts Zdfjls',
        'remark': '快管可给始风性段养按命始斯每满前。己研理内又之进团节劳克选。至表团元着位现教张计离出花越海。队京受力军温素美两文按先正等铁同天年。活具济问习打才新回统新林心保保比。为问装确们取条身速行龙五特会。',
        'desc': '才战中质非收一起革通般热然量计。'
      }, {
        'name': '宋杰',
        'tel': '15188915603',
        'age': 36,
        'count': 3,
        'birthday': '2005-03-14',
        'datetime': '2004-10-03 07:13:05',
        'now': '2020-01-20 15:20:41',
        'email': 'n.plr@brsmpginqy.pro',
        'city': '上海市',
        'color': '#c5f279',
        'id': '710000201308126733',
        'image': 'http://dummyimage.com/200x200',
        'tag': 'Ptdqtcrkm Igocxdjqjp Lkfjy Fpeyuklnf Wlieg Ehbrqry',
        'remark': '自层都阶具比流其那复定展三着研切组。满我说江八题战反光领用分直府质。府别温织制会满来别治理取京况成位书。今法时好命老题金般白通流入及维物联。还易之还带或县美清很放话运约学求该。',
        'desc': '参住广林候叫数政细名看流。'
      }, {
        'name': '常超',
        'tel': '18612834970',
        'age': 21,
        'count': 6,
        'birthday': '2008-07-19',
        'datetime': '1993-09-06 05:52:43',
        'now': '2020-01-20 15:20:41',
        'email': 'j.bmfrczmbu@svycjmkcrb.yu',
        'city': '通化市',
        'color': '#f279e8',
        'id': '350000198507144780',
        'image': 'http://dummyimage.com/200x200',
        'tag': 'Cyirvtw Dnfs Kwqppgoho Qcxgju Dphfsxr',
        'remark': '别等江打定难听且边由话治于因离照称。小都口太两二本育必商们飞达八。以大领入状加着会八拉须己引量照海。好听八成非声头期铁众处构。日变这技热动一算属四和全些才到中统。它外青认界联山常切通小比眼政。',
        'desc': '还调近正起支建年大需天来们先。'
      }, {
        'name': '魏芳',
        'tel': '16451223997',
        'age': 56,
        'count': 5,
        'birthday': '1988-01-23',
        'datetime': '2004-02-01 22:08:49',
        'now': '2020-01-20 15:20:41',
        'email': 'a.nhwryorli@gtxaeotn.tg',
        'city': '日喀则地区',
        'color': '#f28c79',
        'id': '420000201805155966',
        'image': 'http://dummyimage.com/200x200',
        'tag': 'Busvibqby Kypomjimt Fqdjtbfwk Xtvs Mhkwe Vzrgsknd',
        'remark': '部车较基型使原史革导连京来单层。并组同装斯装点规证你后易众见农达队。记整总在养组能习律者打特。级把展用明格值质且之许元养不科温。学系力易员何特六场生太为。',
        'desc': '效应义务千多积史米参看酸更。'
      }, {
        'name': '易娟',
        'tel': '15434122655',
        'age': 54,
        'count': 10,
        'birthday': '1998-01-17',
        'datetime': '2006-12-14 06:38:08',
        'now': '2020-01-20 15:20:41',
        'email': 'b.uiqgpc@sncckvk.bb',
        'city': '肇庆市',
        'color': '#7988f2',
        'id': '370000199602254433',
        'image': 'http://dummyimage.com/200x200',
        'tag': 'Ksgjiw Budxwnkm Xlahhgm Qei',
        'remark': '天发改级己影至少候九相当克格系名片。变报至因龙好二己存处色极手百放。术至前广局小认族除然报府同。表族统角规需济共主命带出各领或。',
        'desc': '本属称备美验代列圆体义山。'
      }],
      list1: [
        {
          url:
            'https://zos.alipayobjects.com/rmsportal/ODTLcjxAfvqbxHnVXCYX.png',
          label: '头像提示1'
        },
        {
          url:
            'https://zos.alipayobjects.com/rmsportal/ODTLcjxAfvqbxHnVXCYX.png',
          label: '头像提示2'
        },
        {
          url:
            'https://zos.alipayobjects.com/rmsportal/ODTLcjxAfvqbxHnVXCYX.png',
          label: '头像提示3'
        },
        {
          url:
            'https://zos.alipayobjects.com/rmsportal/ODTLcjxAfvqbxHnVXCYX.png',
          label: '头像提示4'
        },
        {
          url:
            'https://zos.alipayobjects.com/rmsportal/ODTLcjxAfvqbxHnVXCYX.png',
          label: '头像提示5'
        },
        {
          url:
            'https://zos.alipayobjects.com/rmsportal/ODTLcjxAfvqbxHnVXCYX.png',
          label: '头像提示6'
        }
      ],
      draggableValue: {
        handle: null
      }
    }
  },
  methods: {
    close () {
      this.$emit('input', false)
    },
    handleEmoji (emoji) {
      this.messageInfo += emoji
    },
    getData () {
      // this.$api.USER_LIST().then(res => {
      //   this.list2 = res.list
      // })
    }
  },
  mounted () {
    this.getData()
    this.draggableValue.handle = this.$refs.handle
  }
}
</script>

<style lang="less" scoped>
@import '../../assets/styles/var.less';
.artiely-chat {
  height: 500px;
  width: 900px;
  position: fixed;
  right: 0;
  z-index: 999;
  bottom: 20px;
  border-radius: 8px;
  box-shadow: 0px 0px 22px rgba(0, 0, 0, 0.1);
  display: flex;
  background: fade(@background-color-base, 90%);
  flex-direction: column;
  overflow: hidden;
  .chat-top {
    height: 50px;
    padding-top: 10px;
    padding-left: 10px;
    display: flex;
    .top-l {
      width: 180px;
      display: flex;
      line-height: 40px;
    }
    .top-m {
      flex: 1;
      font-size: 18px;
      color: @text-color;
      line-height: 40px;
    }
    .top-r {
      width: 180px;
      text-align: center;
      line-height: 40px;
    }
    .icon-smallscreen {
      position: absolute;
      top: -10px;
      right: 0;
      font-size: 24px;
      cursor: pointer;
    }
  }
  .chat-container {
    display: flex;
    height: 450px;
    flex: 1;
  }
  .input-action {
    display: flex;
    padding-left: 10px;
    .iconfont {
      font-size: 24px;
      margin: 0 3px;
    }
  }
  .custom-list {
    display: flex;
    border-top: 1px solid @background-color-base;
    padding: 10px 0;
    .userinfo {
      display: flex;
      flex: 1;
      flex-direction: column;
      padding-left: 5px;
      .name {
        font-size: 14px;
        color: @text-color-secondary;
        margin: 0;
      }
      .tel {
        font-size: 10px;
        color: #999;
      }
    }
    .message-count {
      transform: scale(0.8);
    }
  }
  .chat-custom-wrapper {
    display: flex;
    width: 180px;
    padding-left: 10px;
    flex-direction: column;
    .custom-action {
      height: 50px;
    }
  }
  .custom-wrapper {
    flex: 1;
    overflow-y: scroll;
  }
  .chat-content-wrapper {
    flex: 2;
    background: rgba(@layout-body-background, 0.9);
    display: flex;
    flex-direction: column;
    .chat-list {
      flex: 3;
      display: flex;
      flex-direction: column;
      .chat-body {
        flex: 1;
        display: flex;
        flex-direction: column;
        .avatar {
          width: 40px;
        }
        .left {
          display: flex;
          padding-left: 5px;
          padding-right: 20px;
          padding-top: 40px;
          justify-content: flex-start;
          .message-box {
            flex: 1;
            display: flex;
            justify-content: flex-start;
          }
          .message {
            margin-left: 10px;
            background: rgba(#1690ff, 0.5);
            color: #fff;
            padding: 6px;
            border-radius: 4px;
            position: relative;
          }
        }
        .right {
          display: flex;
          padding-left: 20px;
          padding-right: 5px;
          padding-top: 40px;
          justify-content: flex-end;
          .message-box {
            flex: 1;
            display: flex;
            justify-content: flex-end;
          }
          .message {
            margin-right: 10px;
            background: rgba(#bbb);
            color: #fff;
            padding: 6px;
            border-radius: 4px;
            position: relative;
          }
        }
      }
    }
    .chat-input {
      flex: 1;
      background: @layout-body-background;
      display: flex;
      flex-direction: column;
      .input-field {
        flex: 1;
        border: none;
        outline: none;
        box-shadow: none;
        overflow-y: scroll;
      }
      .send {
        text-align: right;
        padding-right: 20px;
        .iconfont {
          font-size: 24px;
          cursor: pointer;
        }
      }
    }
  }
  .message-list {
    width: 180px;
    overflow-y: scroll;
    .message-item {
      background: @body-background;
      cursor: pointer;
      padding: 3px 8px;
      border-radius: 4px;
      margin: 5px 10px;
      &:hover {
        background: darken(@body-background,10%);
      }
    }
  }
}
</style>
