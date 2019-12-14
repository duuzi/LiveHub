// pages/user/user.js
import WxValidate from '../../utils/WxValidate.js'
const util = require('../../utils/util.js');
const api = require('../../config/api.js');
const app = getApp()
Page({

  /**
   * 页面的初始数据
   */
  data: {
    accountNo: '',
    accountName: '',
    nickName: '',
    phoneNumber:''
  },
  getUserDetail: function () {
    util.request(api.GetUser).then(res => {
      
      this.setData({
        // accountNo: res.data.accountNo,
        // accountName: res.data.accountName,
        phoneNumber: res.data.phoneNumber
      })
    })
  },

  initValidate() {
    const rules = {
      phoneNumber: {
        required: true,
        tel: true,
      },
      // accountNo: {
      //   required: true,
      //   maxlength: 10
      // },
      // accountName: {
      //   required: true,
      //   maxlength: 10
      // },
    }
    const messages = {
      phoneNumber: {
        required: '请输入手机号',
        tel: '请输入正确的手机号码',
      },
      // accountNo: {
      //   required: '请输入工号',
      //   maxlength: '不能超过10个字'
      // },
      // accountName: {
      //   required: '请输入姓名',
      //   maxlength: '不能超过10个字'
      // },
    }
    this.WxValidate = new WxValidate(rules, messages)
  },
  showModal(error) {
    wx.showModal({
      content: error.msg,
      showCancel: false,
    })
  },
  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
    this.initValidate()
    this.getUserDetail()
    //let _this = this
    // wx.getSetting({
    //   success(res) {
    //     if (res.authSetting['scope.userInfo']) {
    //       wx.getUserInfo({
    //         success: function (res) {
    //           _this.setData({
    //             nickName: res.userInfo.nickName
    //           })
    //           wx.request({
    //             url: config.host + '/Account/GetUser?openid=' + app.globalData.openId,
    //             success: function (res) {
    //               if (res.data.success) {
    //                 _this.setData({
    //                   accountNo: res.data.data.accountNo,
    //                   accountName: res.data.data.accountName,
    //                 })
    //               }
    //             },
    //             fail: function () {
    //               wx.showToast({
    //                 title: '获取用户信息失败,请检查网络连接',
    //               })
    //             }
    //           })
    //         },
    //         fail: app.toLogin
    //       })
    //     } else {
    //       wx.showToast({
    //         title: '请先登录!',
    //       })
    //     }
    //   }
    // })


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
  formSubmit: function (e) {
    const param = e.detail.value
    if (!this.WxValidate.checkForm(param)) {
      const error = this.WxValidate.errorList[0]
      this.showModal(error)
      return false
    }
    let phoneNumber = e.detail.value.phoneNumber
    let accountNo = e.detail.value.accountNo
    let accountName = e.detail.value.accountName
    console.log('提交表单e' + e)
    let params = {
      "PhoneNumber": phoneNumber,
    }
    util.request(api.UpdateUser, params,'POST').then(res=>{
      let item = JSON.parse(wx.getStorageSync('userInfo'));
      item.accountName = accountName
      item.accountNo = accountNo
      wx.setStorageSync('userInfo', JSON.stringify(item));
      
      wx.switchTab({
        url: '/pages/user/index/index',
      })
      wx.showToast({
        duration:1500,
        title: '更新成功!',
      })
    })




    // wx.request({
    //   url: config.host + '/Account/UpdateOrAddUser',
    //   data: {
    //     "OpenId": app.globalData.openId,
    //     "AccountNo": accountNo,
    //     "AccountName": accountName,
    //     "NikcName": this.data.nickName
    //   },
    //   success: function (res) {
    //     if (res.data.success) {
    //       wx.switchTab({
    //         url: '/pages/index/index',
    //       })
    //       wx.showToast({
    //         duration: 2000,
    //         title: '修改成功!',
    //       })
    //     } else {
    //       wx.showToast({
    //         duration: 2000,
    //         title: res.data.msg,
    //       })
    //     }
    //   },
    //   fail: function () {

    //   }
    // })

  }
})