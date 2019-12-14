// pages/user/download/download.js
const util = require('../../../utils/util.js');
const api = require('../../../config/api.js');

Page({

  /**
   * 页面的初始数据
   */
  data: {

  },
  downloadExcel: function (event) {
    wx.showToast({
      title: '生成中',
      mask: true,
      icon: 'loading',
    })
    wx.downloadFile({
      url: api.DownloadExcel + '?DownloadDateType=' + event.target.dataset.dtype,
      // data: {
      //   DownloadDateType: event.target.dataset.dtype
      //   },
      header: {
        'Authorization': 'Bearer ' + wx.getStorageSync('token'),
        'X-Requested-With': 'XMLHttpRequest'
      },
      success: function (res) {
        debugger
        if (res.statusCode == 200) {
          // wx.saveFile({
          //   tempFilePath: res.tempFilePath,
          //   success: function (res) {
          //     console.log("下载成功")
          //   }
          // })
          wx.openDocument({
            filePath: res.tempFilePath,
            fileType: 'xlsx',
            success: function (res) {
              console.log(res)
              console.log('打开文档成功')
            },
            fail: function (res) {
              console.log("fail");
              console.log(res)
            },
            complete: function (res) {
              console.log("complete");
              console.log(res)
            }
          })
        }else{
          //util.showErrorToast("未授权!")
        }
      },
      fail: function (res) { },
      complete: function (res) {
        wx.hideToast()
      },
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