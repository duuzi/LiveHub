<template>
<!--pages/user/download/download.wxml-->
<view class="container">
  <view class="cu-bar bg-white">
    <view class="action">
      <text class="cuIcon-title text-green"></text>
      <text class="text-grey">点击按钮下载对应时间段打卡信息</text>
    </view>
  </view>
  <view class="box">
    <!-- <view class="cu-bar btn-group">
    <button class="cu-btn bg-green shadow-blur round lg">保存</button>
  </view> -->
    <view class="cu-bar btn-group">
      <button class="cu-btn bg-green shadow-blur" data-dtype="0" @tap="downloadExcel">昨日</button>
      <button class="cu-btn bg-green shadow-blur" data-dtype="1" @tap="downloadExcel">今日</button>
    </view>
    <view class="cu-bar btn-group">
      <button class="cu-btn bg-green shadow-blur" data-dtype="2" @tap="downloadExcel">上周</button>
      <button class="cu-btn bg-green shadow-blur" data-dtype="3" @tap="downloadExcel">本周</button>
      <!-- <button class="cu-btn text-green line-green shadow">上月</button> -->
    </view>
    <view class="cu-bar btn-group">
      <button class="cu-btn bg-green shadow-blur" data-dtype="4" @tap="downloadExcel">上月</button>
      <button class="cu-btn bg-green shadow-blur" data-dtype="5" @tap="downloadExcel">本月</button>
    </view>
    <!-- <view class="cu-bar btn-group">
    <button class="cu-btn bg-green shadow-blur round">保存</button>
    <button class="cu-btn bg-blue shadow-blur round">提交</button>
  </view> -->
  </view>
</view>
</template>

<script>
// pages/user/download/download.js
const util = require("../../../utils/util.js");
const api = require("../../../config/api.js");

export default {
  data() {
    return {};
  },

  components: {},
  props: {},

  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {},

  /**
   * 生命周期函数--监听页面初次渲染完成
   */
  onReady: function () {},

  /**
   * 生命周期函数--监听页面显示
   */
  onShow: function () {},

  /**
   * 生命周期函数--监听页面隐藏
   */
  onHide: function () {},

  /**
   * 生命周期函数--监听页面卸载
   */
  onUnload: function () {},

  /**
   * 页面相关事件处理函数--监听用户下拉动作
   */
  onPullDownRefresh: function () {},

  /**
   * 页面上拉触底事件的处理函数
   */
  onReachBottom: function () {},

  /**
   * 用户点击右上角分享
   */
  onShareAppMessage: function () {},
  methods: {
    downloadExcel: function (event) {
      wx.showToast({
        title: '生成中',
        mask: true,
        icon: 'loading'
      });
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
          debugger;

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
                console.log(res);
                console.log('打开文档成功');
              },
              fail: function (res) {
                console.log("fail");
                console.log(res);
              },
              complete: function (res) {
                console.log("complete");
                console.log(res);
              }
            });
          } else {//util.showErrorToast("未授权!")
          }
        },
        fail: function (res) {},
        complete: function (res) {
          wx.hideToast();
        }
      });
    },
    setData: function (obj, callback) {
      let that = this;
      let keys = [];
      let val, data;
      Object.keys(obj).forEach(function (key) {
        keys = key.split('.');
        val = obj[key];
        data = that.$data;
        keys.forEach(function (key2, index) {
          if (index + 1 == keys.length) {
            that.$set(data, key2, val);
          } else {
            if (!data[key2]) {
              that.$set(data, key2, {});
            }
          }

          data = data[key2];
        });
      });
      callback && callback();
    }
  }
};
</script>
<style>
@import "./download.css";
</style>