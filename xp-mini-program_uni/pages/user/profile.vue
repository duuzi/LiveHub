<template>
<!--pages/user/user.wxml-->
<view>
<view class="cu-bar bg-white">
    <view class="action">
      <text class="cuIcon-title text-red"></text>
      <text class="text-grey">这个暂时不用填写哦</text>
    </view>
  </view>
<form @submit="formSubmit" @reset="formReset">
  <!-- <view class="section section_gap">
    <view class="section__title">switch</view>
    <switch name="switch"/>
  </view> -->
  <!-- <view class="section section_gap">
    <view class="section__title">slider</view>
    <slider name="slider" show-value ></slider>
  </view> -->

  <view class="cu-form-group margin-top">
    <view class="title">账号</view>
    <input v-model="accountNo" :disabled="true"></input>
  </view>
  <view class="cu-form-group margin-bottom">
    <view class="title">昵称</view>
    <input v-model="nickName" placeholder="请输入昵称"></input>
  </view>
  <!-- <view class="section section_gap">
    <view class="section__title">radio</view>
    <radio-group name="radio-group">
      <label><radio value="radio1"/>radio1</label>
      <label><radio value="radio2"/>radio2</label>
    </radio-group>
  </view>
  <view class="section section_gap">
    <view class="section__title">checkbox</view>
    <checkbox-group name="checkbox">
      <label><checkbox value="checkbox1"/>checkbox1</label>
      <label><checkbox value="checkbox2"/>checkbox2</label>
    </checkbox-group>
  </view> -->
  <view class="cu-bar btn-group">
    <button class="cu-btn bg-green shadow-blur round" formType="reset">清 空</button>
    <button class="cu-btn bg-blue shadow-blur round" formType="submit">保 存</button>
    
  </view>
</form>
</view>
</template>

<script>
// pages/user/user.js
import WxValidate from "../../utils/WxValidate";
const util = require("../../utils/util.js");
const api = require("../../config/api.js");
const app = getApp().globalData;

export default {
  data() {
    return {
      accountNo: '',
      accountName: '',
      nickName: ''
    };
  },

  components: {},
  props: {},

  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
    this.initValidate();
    this.getUserDetail(); //let _this = this
  },

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
    getUserDetail: function () {
      util.request(api.GetUser).then(res => {
		  this.nickName = res.data.nickName||''
		  this.accountNo = res.data.accountNo||''
      });
    },

    initValidate() {
      const rules = {
        accountNo: {
          required: true,
          maxlength: 10
        },
        accountName: {
          required: true,
          maxlength: 10
        }
      };
      const messages = {
        nickName: {
          required: '请输入昵称',
          maxlength: '不能超过10个字'
        }
      };
      this.WxValidate = new WxValidate(rules, messages);
    },

    showModal(error) {
      wx.showModal({
        content: error.msg,
        showCancel: false
      });
    },

    formSubmit: function (e) {
      const param = e.detail.value;

      if (!this.WxValidate.checkForm(param)) {
        const error = this.WxValidate.errorList[0];
        this.showModal(error);
        return false;
      }

      let accountNo = e.detail.value.accountNo;
      let accountName = e.detail.value.accountName;
      console.log('提交表单e' + e);
      let params = {
        "AccountNo": accountNo,
        "AccountName": accountName
      };
      util.request(api.UpdateUser, params, 'POST').then(res => {
        let item = JSON.parse(wx.getStorageSync('userInfo'));
        item.accountName = accountName;
        item.accountNo = accountNo;
        wx.setStorageSync('userInfo', JSON.stringify(item));
        wx.switchTab({
          url: '/pages/user/index/index'
        });
        wx.showToast({
          duration: 1500,
          title: '更新成功!'
        });
      }); // wx.request({
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
</style>