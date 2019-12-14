<template>
<!--logs.wxml-->
<view class="container log-list">
  <block v-for="(log, index) in logs" :key="index">
    <text class="log-item">{{index + 1}}. {{log}}</text>
  </block>
</view>
</template>

<script>
//logs.js
const util = require("../../utils/util.js");

export default {
  data() {
    return {
      logs: []
    };
  },

  components: {},
  props: {},
  onLoad: function () {
    this.setData({
      logs: (wx.getStorageSync('logs') || []).map(log => {
        return util.formatTime(new Date(log));
      })
    });
  },
  methods: {
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
@import "./logs.css";
</style>