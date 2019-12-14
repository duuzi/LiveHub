<template>
<!--pages/auth/auth.wxml-->
<view class="container">
<button v-if="canIUse" open-type="getUserInfo" @getuserinfo="bindGetUserInfo">授权登录</button>
<view v-else>请升级微信版本</view>
<!-- <button open-type="getUserInfo" bindgetuserinfo="getUserInfo"> 点击授权 </button> -->
</view>
</template>

<script>
// pages/auth/auth.js
import config from "../../utils/const";
const app = getApp().globalData;

export default {
  data() {
    return {
      canIUse: wx.canIUse('button.open-type.getUserInfo'),
      userInfo: "",
      hasUserInfo: false
    };
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
    bindGetUserInfo: function (e) {
      console.log(e);
      getApp().globalData.userInfo = e.detail.userInfo;
      this.setData({
        userInfo: e.detail.userInfo,
        hasUserInfo: true
      });
      let openid = getApp().globalData.openId;
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
                console.log(res.data.msg);
                wx.showToast({
                  title: '授权成功！'
                });
                wx.switchTab({
                  url: '/pages/business/business'
                });
              }
            });
          } else {
            wx.showToast({
              title: '授权成功！'
            });
            wx.switchTab({
              url: '/pages/business/business'
            });
            console.log("已注册!");
          }
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
@import "./auth.css";
</style>