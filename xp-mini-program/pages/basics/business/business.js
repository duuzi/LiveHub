// pages/business/business.js
import WxValidate from '../../../utils/WxValidate.js'
const util = require('../../../utils/util.js');
const api = require('../../../config/api.js');

const app = getApp()
Page({

  /**
   * 页面的初始数据
   */
  data: {
    bsTypes: [{
      id: 0,
      name: '出差'
    },
    {
      id: 1,
      name: '返程'
    },
    {
      id: 2,
      name: '中转'
    }
    ],
    times: [{
      id: 0,
      name: '上午'
    },
    {
      id: 1,
      name: '下午'
    }
    ],
    array: ['美国', '中国', '巴西', '日本'],
    fromIndex: -1,
    toIndex: -1,
    timeIndex: null,
    loading: false,
    disabled: false
  },
  bindToChange: function (e) {
    this.setData({
      toIndex: e.detail.value
    })
  },
  bindTimeChange: function (e) {
    this.setData({
      timeIndex: e.detail.value
    })
  },
  formReset: function (e) {
    console.log("已重置")
  },
  formSubmit: function (e) {
    if (!wx.getStorageSync('token')){
      util.showErrorToast("请先登录!")
    }
    let user = JSON.parse(wx.getStorageSync('userInfo')||'');
    if (user.accountNo == null || user.accountNo==''){
      util.showErrorToast("请先完善工号!")
      return
    }
    const params = e.detail.value
    if (!this.WxValidate.checkForm(params)) {
      const error = this.WxValidate.errorList[0]
      this.showModal(error)
      return false
    }
    let lcfrom = e.detail.value.from
    let lcto = e.detail.value.to
    let lcbsType = e.detail.value.bsType
    let lctime = e.detail.value.time
    let param = {
      "OpenId": app.globalData.openId,
      "From": lcfrom,
      "To": lcto,
      "AMorPM": lctime,
      "BusinessType": lcbsType
    }
    this.setData({
      disabled: true,
      loading: true
    })
    util.request(api.SignIn, param, 'POST').then(res => {
      wx.showToast({
        title: '签到成功!',
      })
      this.setData({
        from: '',
        to: '',
        bsType: 0,
        time: 0,
        disabled: false,
        loading: false
      })
    }).catch(res => {
      this.setData({
        disabled: false,
        loading: false
      })
    })
    // wx.request({
    //   url: config.host +'/Business/SignIn',
    //   data:{
    //     "OpenId": app.globalData.openId,
    //     "From": lcfrom,
    //     "To": lcto,
    //     "AMorPM": lctime,
    //     "BusinessType": lcbsType
    //   },
    //   success:function(res){
    //     if(res.data.success){
    //       wx.showToast({
    //         title:'签到成功'
    //       })
    //     }else{
    //       wx.showToast({
    //         title: res.data.msg||'签到失败'
    //       })
    //     }
    //   },
    //   fail:function(res){
    //     wx.showToast({
    //       title: '签到失败'
    //     })
    //   }
    // })
  },
  onLoad: function (options) {
    this.initValidate()
  },

  /**
   * 生命周期函数--监听页面初次渲染完成
   */
  onReady: function () {

  },

  /**
   * 生命周期函数--监听页面显示
   */
  onShow: function () {
    // wx.getSetting({
    //   success(res) {
    //     if (res.authSetting['scope.userInfo']) {
    //       console.log("userInfo:" + res.authSetting['scope.userInfo'])
    //       // 已经授权，可以直接调用 getUserInfo 获取用户信息
    //     }else{
    //       app.toLogin()
    //     }
    //   }
    // })

  },
  /**
   * 生命周期函数--监听页面隐藏
   */
  onHide: function () {

  },

  /**
   * 生命周期函数--监听页面卸载
   */
  onUnload: function () {

  },
  /**
   * 页面相关事件处理函数--监听用户下拉动作
   */
  onPullDownRefresh: function () {

  },
  /**
   * 页面上拉触底事件的处理函数
   */
  onReachBottom: function () {

  },
  /**
   * 用户点击右上角分享
   */
  onShareAppMessage: function () {

  },

  initValidate() {
    const rules = {
      from: {
        required: true,
        maxlength: 30
      },
      to: {
        required: true,
        maxlength: 30
      },
      bsType: {
        required: true,
      },
      time: {
        required: true,
      }
    }
    const messages = {
      from: {
        required: '请输入出发地',
        maxlength: '出发地不能超过30个字'
      },
      to: {
        required: '请输入目的地',
        maxlength: '目的地不能超过30个字'
      },
      bsType: {
        required: '请选择类型',
      },
      time: {
        required: '请选择时间',
      }
    }
    this.WxValidate = new WxValidate(rules, messages)
  },
  showModal(error) {
    wx.showModal({
      content: error.msg,
      showCancel: false,
    })
  },
})