const util = require('../../../utils/util.js');
const api = require('../../../config/api.js');
const user = require('../../../services/user.js');
const app = getApp();

Page({
  data: {
    userInfo: {},
    showLoginDialog: false,
    showDownload:false
  },
  onLoad: function (options) {
    // 页面初始化 options为页面跳转所带来的参数
  },
  onReady: function () {

  },
  onShow: function () {
    let userinfo = JSON.parse(wx.getStorageSync('userInfo')||null)
    let flag = false
    
    if (userinfo == null || userinfo == '' || !wx.getStorageSync('token')){
      userinfo = {
        nickName: '点击登录',
        avatarUrl: '/../images/touxiang.png'
      }
    }
    this.setData({
      userInfo: userinfo,
      showDownload: userinfo.roleName=='助理'
      //userInfo: userinfo||app.globalData.userInfo,
    });
  },
  onHide: function () {
    // 页面隐藏

  },
  onUnload: function () {
    // 页面关闭
  },

  onUserInfoClick: function () {
    if (wx.getStorageSync('token')) {

    } else {
      this.showLoginDialog();
    }
  },

  onUserLogOut: function () {
    let that = this
    if (wx.getStorageSync('token')) {
      wx.showModal({
        title: '登出',
        content: '是否登出系统',
        success(res) {
          if (res.confirm) {
            wx.clearStorageSync();
            that.onShow()
            console.log('登出系统')
          } else if (res.cancel) {
          }
        }
      })
    } else {
      //this.showLoginDialog();
    }
  },
  showLoginDialog() {
    this.setData({
      showLoginDialog: true
    })
  },

  onCloseLoginDialog() {
    this.setData({
      showLoginDialog: false
    })
  },

  onDialogBody() {
    // 阻止冒泡
  },

  onWechatLogin(e) {
    if (e.detail.errMsg !== 'getUserInfo:ok') {
      if (e.detail.errMsg === 'getUserInfo:fail auth deny') {
        return false
      }
      wx.showToast({
        title: '微信登录失败',
      })
      return false
    }
    util.login().then((res) => {
      return util.request(api.AuthLoginByWeixin, {
        code: res,
        userInfo: JSON.stringify(e.detail.userInfo)
      }, 'POST');
    }).then((res) => {
      console.log(res)
      // if (res.errno !== 0) {
      if (!res.success) {
        wx.showToast({
          title: '微信登录失败',
        })
        return false;
      }
      // let showdownload = false
      // if (res.data.userInfo.role!=null){
      //   if (res.data.userInfo.role.roleName=='助理'){
      //     showdownload = true
      //   }
      // }
      // 设置用户信息
      this.setData({
        userInfo: res.data.userInfo,
        showLoginDialog: false,
        //菜单权限
        showDownload: (res.data.userInfo != null && res.data.userInfo != '') ? res.data.userInfo.roleName=='助理':false
      });
      app.globalData.userInfo = res.data.userInfo;
      app.globalData.token = res.data.token;
      wx.setStorageSync('userInfo', JSON.stringify(res.data.userInfo));
      wx.setStorageSync('token', res.data.token);
    }).catch((err) => {
      console.log(err)
    })
  },

  // onOrderInfoClick: function (event) {
  //   wx.navigateTo({
  //     url: '/pages/ucenter/order/order',
  //   })
  // },

  onSectionItemClick: function (event) {

  },
  showQrcode() {
    wx.previewImage({
      urls: ['https://6465-dev-miew-1300831810.tcb.qcloud.la/donate.jpg?sign=2d1957bd731d2ea0979268c96a5289b9&t=1575283575']   
    })
  },
  // TODO 移到个人信息页面
  exitLogin: function () {
    wx.showModal({
      title: '',
      confirmColor: '#b4282d',
      content: '退出登录？',
      success: function (res) {
        if (res.confirm) {
          wx.removeStorageSync('token');
          wx.removeStorageSync('userInfo');
          wx.switchTab({
            url: '/pages/index/index'
          });
        }
      }
    })
  }
})