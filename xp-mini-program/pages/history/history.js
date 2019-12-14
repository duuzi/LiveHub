// pages/history/history.js
const config = require('../../utils/const.js')
const util = require('../../utils/util.js');
const api = require('../../config/api.js');
const app = getApp()
Page({

  /**
   * 页面的初始数据
   */
  data: {
    title: '加载中...', // 状态
    list: [],
    loading: true // 显示等待框
  },
  onLoad: function(options) {
    let _this = this
    this.getList()

    // wx.request({
    //   url: config.host + '/Business/GetBusinessList?openid=' + app.globalData.openId,
    //   method:'GET',
    //   header:{
    //     'content-type': 'json' // 默认值
    //   },
    //   success:function(result){
    //     console.log(result);
    //     result.data.data.map(_=>{
    //       _.businessTypeName = config.BusinessType.find(item => item.id == _.businessType).name;
    //       _.amOrPmDesc = config.AmOrPm.find(item => item.id == _.businessType).name;
    //       return _
    //     })
    //     _this.setData({
    //       list:result.data.data,
    //       loading:false
    //     })
    //   }
    // })
  },
  onReady: function() {},
  onShow: function() {},
  onHide: function() {

  },
  onUnload: function() {},
  onPullDownRefresh: function() {

  },
  onReachBottom: function() {

  },
  onShareAppMessage: function() {
    
  },
  getList: function() {
    wx.showLoading({
      title: '加载中',
    })
    util.request(api.GetSignInList).then(res => {
      if (res.data != null && res.data.length > 0) {
        res.data.map(_ => {
          _.signDate = util.formatDate(_.signDate)
          return _
        })
        this.setData({
          list: res.data,
        })
        wx.hideLoading()
      }
    }).catch(res=>{
      wx.hideLoading()
    })
  }
})