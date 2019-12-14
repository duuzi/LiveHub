<template>
<view class="contianer">
  <view class="cu-bar bg-white">
    <view class="action">
      <text class="cuIcon-title text-blue"></text>
      <text class="text-grey">仅显示最近15天的打卡记录</text>
    </view>
  </view>
  <view class="cu-timeline" v-for="(item, id) in list" :key="id">
    <view class="cu-item">
      <view class="content">
        <view class="cu-capsule radius">
          <view class="cu-tag bg-cyan">{{item.signDate}}</view>
        </view>
        <view>
          <text class="text-grey">日记: </text><text class="text-shadow">{{item.signText}}</text>
        </view>
      </view>
    </view>
  </view>

</view>
</template>

<script>
// pages/history/history.js
const config = require("../../utils/const.js");
const util = require("../../utils/util.js");
const api = require("../../config/api.js");
const app = getApp().globalData;

export default {
  data() {
    return {
      title: '加载中...',
      // 状态
      list: [],
      loading: true // 显示等待框

    };
  },

  components: {},
  props: {},
  onLoad: function (options) {
    let _this = this;

    this.getList(); // wx.request({
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
  onReady: function () {},
  onShow: function () {},
  onHide: function () {},
  onUnload: function () {},
  onPullDownRefresh: function () {},
  onReachBottom: function () {},
  onShareAppMessage: function () {},
  methods: {
    getList: function () {
      wx.showLoading({
        title: '加载中'
      });
      util.request(api.GetSignInList).then(res => {
        if (res.data != null && res.data.length > 0) {
          res.data.map(_ => {
            _.signDate = util.formatDate(_.signDate);
            return _;
          });
          this.setData({
            list: res.data
          });
          wx.hideLoading();
        }
      }).catch(res => {
        wx.hideLoading();
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
@import "./history.css";
</style>