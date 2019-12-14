// pages/auth/auth.js
import config from '../../utils/const.js'
const app=getApp()
Page({

  /**
   * 页面的初始数据
   */
  data: {
    canIUse: wx.canIUse('button.open-type.getUserInfo')
  },

  bindGetUserInfo: function (e) {
    console.log(e)
    app.globalData.userInfo = e.detail.userInfo
    this.setData({
      userInfo: e.detail.userInfo,
      hasUserInfo: true
    })
    let openid = app.globalData.openId
    wx.request({
      url: config.host + '/Account/IsExitUser?openid=' + openid,
      success: function (res) {
        if (res.data.success == true && res.data.data == false) {
          wx.request({
            url: config.host + '/Account/Regist',
            data: {
              "NickName": e.detail.userInfo.nickName,
              "OpenId": openid
            },
            success: function (res) {
              console.log(res.data.msg)
              wx.showToast({
                title: '授权成功！',
              })
              wx.switchTab({
                url: '/pages/business/business',
              })
            }
          })
        } else {
          wx.showToast({
            title: '授权成功！',
          })
          wx.switchTab({
            url: '/pages/business/business',
          })
          console.log("已注册!")
        }
      }
    })
  },
  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {

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

  }
})