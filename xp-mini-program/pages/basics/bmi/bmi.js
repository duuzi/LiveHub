// pages/basics/bmi/bmi.js
import WxValidate from '../../../utils/WxValidate.js'
Page({

  /**
   * 页面的初始数据
   */
  data: {
    bmi:null,
    ColorList: [{
        title: '偏瘦',
        name:'grey',
        desc: '<=18.4',
        color: '#e54d42'
      },
      {
        title: '正常',
        name: 'green',
        desc: '18.5~23.9',
        color: '#f37b1d'
      },
      {
        title: '过重',
        name: 'orange',
        desc: '24~27.9',
        color: '#fbbd08'
      }, {
        title: '肥胖',
        name: 'red',
        desc: '>=28',
        color: '#39b54a'
      },
    ]
  },
  formSubmit: function (e) {
    const params = e.detail.value
    if (!this.WxValidate.checkForm(params)) {
      const error = this.WxValidate.errorList[0]
      this.showModal(error)
      return false
    }
    let height = e.detail.value.height
    let weight = e.detail.value.weight
    let bmi = weight / (height * height)*10000
    this.setData({
      bmi: bmi.toFixed(1)
    })
  },
  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function(options) {
    this.initValidate()
  },

  /**
   * 生命周期函数--监听页面初次渲染完成
   */
  onReady: function() {

  },

  /**
   * 生命周期函数--监听页面显示
   */
  onShow: function() {

  },

  /**
   * 生命周期函数--监听页面隐藏
   */
  onHide: function() {

  },

  /**
   * 生命周期函数--监听页面卸载
   */
  onUnload: function() {

  },

  /**
   * 页面相关事件处理函数--监听用户下拉动作
   */
  onPullDownRefresh: function() {

  },

  /**
   * 页面上拉触底事件的处理函数
   */
  onReachBottom: function() {

  },
  initValidate() {
    const rules = {
      height: {
        required: true,
        maxlength: 10
      },
      weight: {
        required: true,
        maxlength: 10
      },
      
    }
    const messages = {
      height: {
        required: '请输入身高',
        maxlength: '身高不能超过10个字'
      },
      weight: {
        required: '请输入体重',
        maxlength: '体重不能超过10个字'
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
  /**
   * 用户点击右上角分享
   */
  onShareAppMessage: function() {

  }
})