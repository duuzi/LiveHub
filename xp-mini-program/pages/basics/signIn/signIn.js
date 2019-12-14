// pages/basics/signIn/signIn.js
import WxValidate from '../../../utils/WxValidate.js'
const util = require('../../../utils/util.js');
const api = require('../../../config/api.js');
Page({
  /**
   * 页面的初始数据
   */
  data: {
    signed:false,
    signText:'',
    phtext: '',
    phtext1: [
      '今天是周日(•ิ_•ิ),有啥想说的吗？',
      '今天是周一(/ﾟДﾟ)/，有啥想说的吗？',
      '今天是周二（￣～￣;）,有啥想说的吗？',
      '今天是周三(*•̀ㅂ•́)و,有啥想说的吗？',
      '今天是周四(ฅ´ω`ฅ),有啥想说的吗？',
      '今天是周五๑乛◡乛๑,有啥想说的吗？',
      '今天是周六 (●´∀｀●),有啥想说的吗？',
      ]
  },

  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
    this.setWeekText()
    this.initValidate()
    this.checkSignIn()
  },
  setWeekText() {
    
    let week = new Date().getDay();
    let phText = this.data.phtext1[week]
    this.setData({
      phtext: phText
    })
  },
  checkSignIn(){
    util.request(api.CheckSignIn).then(res => {
      if(res.data.signed){
        this.setData({
          signed: res.data.signed,
          signText: res.data.signtext
        })
      }
    })
  },
  submitSignIn(e){
    const params = e.detail.value
    if (!this.WxValidate.checkForm(params)) {
      const error = this.WxValidate.errorList[0]
      this.showModal(error)
      return false
    }
    util.request(api.AddSignIn, params, 'POST').then(res => {
      wx.showToast({
        title: '签到成功!',
      })
      this.checkSignIn()
    })
  },
  showModal(error) {
    wx.showModal({
      content: error.msg,
      showCancel: false,
    })
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
  initValidate() {
    const rules = {
      signInText: {
        maxlength: 50
      }
    }
    const messages = {
      signInText: {
        maxlength: '不能超过100个字'
      },
    }
    this.WxValidate = new WxValidate(rules, messages)
  },
  /**
   * 用户点击右上角分享
   */
  onShareAppMessage: function () {

  }
})