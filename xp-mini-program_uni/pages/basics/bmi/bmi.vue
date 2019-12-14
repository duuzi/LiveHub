<template>
<view class="page">
  <view class="page__hd">
    <view class="page__title">
      <text class="cuIcon-newshot"></text> 体重指数计算</view>
    <view class="page__desc">
      <text class="cuIcon-info lg"></text> 身体质量指数（BMI，Body Mass Index）是国际上常用的衡量人体肥胖程度和是否健康的重要标准。
    </view>
  </view>
  <view class="grid col-4 padding-sm">
    <view class="padding-sm" v-for="(item, index) in ColorList" :key="index">
      <view :class="'bg-' + item.name + ' radius text-center shadow-blur'">
        <view class="text-lg" style="color:white">{{item.title}}</view>
        <view class="margin-top-sm text-Abc" style="color:white">{{item.desc}}</view>
      </view>
    </view>
  </view>
  <!--页头-->
  <view class="page__bd page__bd_spacing">
    <form @submit="formSubmit" @reset="formReset">

      <view class="cu-form-group">
        <view class="title"><text class="text-grey">身高</text></view>
        <input name="height" :value="height" placeholder="请输入身高(cm)"></input>
      </view>
      <view class="cu-form-group margin-bottom">
        <view class="title"><text class="text-grey">体重</text></view>
        <input name="weight" :value="weight" placeholder="请输入体重(kg)"></input>
      </view>
      <view class="cu-form-group margin-bottom">
        <view class="title"><text class="text-grey">BMI</text></view>
        <input name="bmi" :value="bmi"></input>
      </view>
      <button formType="submit" type="primary"> 计算 </button>
    </form>
  </view>
  <!--主体-->

</view>
</template>

<script>
// pages/basics/bmi/bmi.js
import WxValidate from "../../../utils/WxValidate";

export default {
  data() {
    return {
      bmi: null,
      ColorList: [{
        title: '偏瘦',
        name: 'grey',
        desc: '<=18.4',
        color: '#e54d42'
      }, {
        title: '正常',
        name: 'green',
        desc: '18.5~23.9',
        color: '#f37b1d'
      }, {
        title: '过重',
        name: 'orange',
        desc: '24~27.9',
        color: '#fbbd08'
      }, {
        title: '肥胖',
        name: 'red',
        desc: '>=28',
        color: '#39b54a'
      }]
    };
  },

  components: {},
  props: {},

  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
    this.initValidate();
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
    formSubmit: function (e) {
      const params = e.detail.value;

      if (!this.WxValidate.checkForm(params)) {
        const error = this.WxValidate.errorList[0];
        this.showModal(error);
        return false;
      }

      let height = e.detail.value.height;
      let weight = e.detail.value.weight;
      let bmi = weight / (height * height) * 10000;
      this.setData({
        bmi: bmi.toFixed(1)
      });
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
        }
      };
      const messages = {
        height: {
          required: '请输入身高',
          maxlength: '身高不能超过10个字'
        },
        weight: {
          required: '请输入体重',
          maxlength: '体重不能超过10个字'
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
@import "./bmi.css";
</style>