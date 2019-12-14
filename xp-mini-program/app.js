//app.js
import config from '/utils/const.js'
App({
  onLaunch:function(){
    try {
      this.globalData.userInfo = JSON.parse(wx.getStorageSync('userInfo'));
      this.globalData.token = wx.getStorageSync('token');
    } catch (e) {
      console.log(e);
    }
  },
  globalData: {
    userInfo: {
      nickName: '点击登录',
      avatarUrl:'/images/touxiang.png'
      //avatarUrl: 'http://yanxuan.nosdn.127.net/8945ae63d940cc42406c3f67019c5cb6.png'
    },
    token: '',
    openId: '',
  },
  toLogin: function () {

    // 前往授权登录界面
    wx.navigateTo({
      url: '/pages/auth/auth',
    })
    wx.showToast({
      title: '请先授权!',
      duration: 2000
    })
  },
  checkHasAccountNo: function () {
    if (this.globalData.openId == null || this.globalData.openId == '') {
      return false;
    }
    wx.request({
      url: config.host + '/Account/CheckHasAccountNo?' + this.globalData.openId,
      success: function (res) {
        if (res.data.success && res.data.data) {
          return true
        } else {
          return false
        }
      },
      fail: function () {
        return false
      }
    })
  }
})