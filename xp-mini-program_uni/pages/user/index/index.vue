<template>
<view>
<view class="container">
  <view class="profile-info bg-gradual-blue">
    <image @tap="onUserInfoClick" class="avatar" :src="userInfo.avatarUrl||'../../../static/images/touxiang.png'"></image>
    <view class="info">
      <text class="name" @tap="onUserInfoClick">{{ userInfo.nickName ||userInfo.accountNo ||'点击登录' }}</text>
      <!-- <text class='level' bindtap='onUserInfoClick'></text> -->
    </view>
    <image @tap="onUserLogOut" class="btn" src="../../../static/images/address_right.png"></image>
    <!-- <image bindtap="onUserInfoClick" class='btn' src='/images/address_right.png'></image> -->
  </view>
  <view class="cu-list menu card-menu margin-top-xl margin-bottom-xl shadow-lg radius">
    <view class="cu-item arrow">
      <navigator class="content" url="/pages/user/profile" hover-class="none">
        <text class="cuIcon-profile text-blue"></text>
        <text class="text">我的信息</text>
      </navigator>
    </view>
    <view class="cu-item arrow">
      <navigator class="content" url="/pages/history/history" hover-class="none">
        <text class="cuIcon-calendar text-green"></text>
        <text class="text">打卡记录</text>
      </navigator>
    </view>
    <!-- <view wx:if="{{showDownload}}" class="cu-item arrow">
      <navigator class="content" url="/pages/user/download/download" hover-class="none">
        <text class="cuIcon-down text-grey"></text>
        <text class="text">统计下载</text>
      </navigator>
    </view> -->
    <view class="cu-item arrow">
      <view class="content" @tap="showQrcode">
        <text class="cuIcon-appreciatefill text-red"></text>
        <text class="text">赞赏支持</text>
      </view>
    </view>
  </view>
  <!-- <view class="user-menu">
    <view class="item">
      <navigator url="/pages/user/user" class="a">
        <text class="icon order"></text>
        <text class="txt">我的信息</text>
      </navigator>
    </view>
    <view class="item">
      <navigator url="/pages/history/history" class="a">
        <text class="icon order"></text>
        <text class="txt">打卡记录</text>
      </navigator>
    </view>
  </view> -->

  <!-- <view class="logout" bindtap="exitLogin">退出登录</view> -->
</view>

<view class="dialog-login" v-if="IsshowLoginDialog" @tap="onCloseLoginDialog">
  <view class="dialog-body" @tap.native.stop="onDialogBody">
    <view class="title">请选择登录方式</view>
    <view class="content">
      <button type="primary" open-type="getUserInfo" @getuserinfo="onWechatLogin">微信登录</button>
      <!-- <button open-type="getUserInfo" lang="zh_CN" bindgetuserinfo="onWechatLogin">手机号登录</button> -->
    </view>
  </view>
</view>
</view>
</template>

<script>
const util = require("../../../utils/util.js");
const api = require("../../../config/api.js");
const user = require("../../../services/user.js");

export default {
  data() {
    return {
      userInfo: {},
      IsshowLoginDialog: false,
      showDownload: false
    };
  },

  components: {},
  props: {},
  onLoad: function (options) {// 页面初始化 options为页面跳转所带来的参数
  },
  onReady: function () {},
  onShow: function () {
    let userinfo = JSON.parse(wx.getStorageSync('userInfo') || null);
    let flag = false;
    if (userinfo == null || userinfo == '' || !wx.getStorageSync('token')) {
      userinfo = {
        nickName: '点击登录',
        avatarUrl: '/../images/touxiang.png'
      };
    }

    this.setData({
      userInfo: userinfo,
      showDownload: userinfo.roleName == '助理' //userInfo: userinfo||app.globalData.userInfo,

    });
  },
  onHide: function () {// 页面隐藏
  },
  onUnload: function () {// 页面关闭
  },
  methods: {
    onUserInfoClick: function () {
      if (wx.getStorageSync('token')) {} else {
        uni.navigateTo({
        	url: '../login',
			animationType: 'slide-in-bottom',
			animationDuration: 300
        });
		//this.showLoginDialog();
      }
    },
    onUserLogOut: function () {
      let that = this;

      if (wx.getStorageSync('token')) {
        wx.showModal({
          title: '登出',
          content: '是否登出系统',

          success(res) {
            if (res.confirm) {
              wx.clearStorageSync();
              // that.onShow();
              console.log('登出系统');
            } else if (res.cancel) {}
          }

        });
      } else {//this.showLoginDialog();
      }
    },

    showLoginDialog() {
      this.setData({
        IsshowLoginDialog: true
      });
    },

    onCloseLoginDialog() {
      this.setData({
        IsshowLoginDialog: false
      });
    },

    onDialogBody() {// 阻止冒泡
    },

    onWechatLogin(e) {
      if (e.detail.errMsg !== 'getUserInfo:ok') {
        if (e.detail.errMsg === 'getUserInfo:fail auth deny') {
          return false;
        }

        wx.showToast({
          title: '微信登录失败'
        });
        return false;
      }

      util.login().then(res => {
        return util.request(api.AuthLoginByWeixin, {
          code: res,
          userInfo: JSON.stringify(e.detail.userInfo)
        }, 'POST');
      }).then(res => {
        console.log(res); // if (res.errno !== 0) {

        if (!res.success) {
          wx.showToast({
            title: '微信登录失败'
          });
          return false;
        } // let showdownload = false
        // if (res.data.userInfo.role!=null){
        //   if (res.data.userInfo.role.roleName=='助理'){
        //     showdownload = true
        //   }
        // }
        // 设置用户信息


        this.setData({
          userInfo: res.data.userInfo,
          IsshowLoginDialog: false,
          //菜单权限
          showDownload: res.data.userInfo != null && res.data.userInfo != '' ? res.data.userInfo.roleName == '助理' : false
        });
        getApp().globalData.userInfo = res.data.userInfo;
        getApp().globalData.token = res.data.token;
        wx.setStorageSync('userInfo', JSON.stringify(res.data.userInfo));
        wx.setStorageSync('token', res.data.token);
      }).catch(err => {
        console.log(err);
      });
    },

    // onOrderInfoClick: function (event) {
    //   wx.navigateTo({
    //     url: '/pages/ucenter/order/order',
    //   })
    // },
    onSectionItemClick: function (event) {},

    showQrcode() {
      wx.previewImage({
        urls: ['https://6465-dev-miew-1300831810.tcb.qcloud.la/donate.jpg?sign=2d1957bd731d2ea0979268c96a5289b9&t=1575283575']
      });
    },

    // TODO 移到个人信息页面
    exitLogin: function () {
      wx.showModal({
        title: '',
        confirmColor: '#b4282d',
        content: '退出登录？',
        success: function (res) {
          if (res.confirm) {
            wx.removeStorageSync('token');
            wx.removeStorageSync('userInfo');
            wx.switchTab({
              url: '/pages/index/index'
            });
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
@import "./index.css";
</style>