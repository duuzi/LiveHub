<template>
	<view>
		<view class="cu-bar bg-white">
			<view class="action">
				<text class="cuIcon-title text-green"></text>
				<text class="text-grey">请填写登录信息</text>
			</view>
		</view>
		<form @submit="formSubmit" @reset="formReset">
			<view class="cu-form-group margin-top">
				<view class="title">账号:</view>
				<input name="accountNo" placeholder="请输入账号"></input>
			</view>
			<view class="cu-form-group margin-bottom">
				<view class="title">密码:</view>
				<input name="password" type="password" placeholder="请输入密码"></input>
			</view>
			<view class="cu-bar btn-group">
				<button class="cu-btn bg-green shadow-blur round" @tap="navToReg">注 册</button>
				<button class="cu-btn bg-blue shadow-blur round" formType="submit">登 录</button>
			</view>
		</form>
	</view>
</template>

<script>
	const util = require("../../utils/util.js");
	const api = require("../../config/api.js");
	export default {
		data() {
			return {}
		},
		methods: {
			formSubmit: function(e) {
				console.log('form发生了submit事件，携带数据为：' + JSON.stringify(e.detail.value))
				var formdata = e.detail.value
				// uni.showModal({
				// 	content: '表单数据内容：' + JSON.stringify(formdata),
				// 	showCancel: false
				// });
				this.onAppLogin(formdata)
			},
			formReset: function(e) {
				console.log('清空数据')
			},
			navToReg() {
				uni.navigateTo({
					url: "./reg"
				})
			},
			onAppLogin(formdata) {
				util.request(api.AuthLoginByApp, JSON.stringify(formdata), 'POST')
					.then(res => {
						console.log(res); // if (res.errno !== 0) {
						debugger
						if (!res.success) {
							wx.showToast({
								title: '登录失败'
							});
							return false;
						}
						// this.setData({
						// 	userInfo: res.data.userInfo,
						// 	IsshowLoginDialog: false,
						// 	//菜单权限
						// 	showDownload: res.data.userInfo != null && res.data.userInfo != '' ? res.data.userInfo.roleName == '助理' : false
						// });
						setTimeout(function() {
							uni.showToast({
								title:"登录成功"
							})
						}, 0);
						getApp().globalData.userInfo = res.data.userInfo;
						getApp().globalData.token = res.data.token;
						wx.setStorageSync('userInfo', JSON.stringify(res.data.userInfo));
						wx.setStorageSync('token', res.data.token);
						uni.reLaunch({
							url:"../home/home"
						})
					}).catch(err => {
						console.log(err);
					});
			},
		}
	}
</script>

<style>
</style>
