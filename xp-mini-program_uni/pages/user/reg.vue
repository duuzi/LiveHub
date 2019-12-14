<template>
	<view>
		<view class="cu-bar bg-white">
			<view class="action">
				<text class="cuIcon-title text-green"></text>
				<text class="text-grey">请填写注册信息</text>
			</view>
		</view>
		<form @submit="formSubmit" @reset="formReset">
			<view class="cu-form-group margin-top">
				<view class="title">邮箱:</view>
				<input name="mailAddress" placeholder="请输入邮箱"></input>
			</view>
			<view class="cu-form-group">
				<view class="title">账号:</view>
				<input name="accountNo" placeholder="请输入账号"></input>
			</view>
			<view class="cu-form-group margin-bottom">
				<view class="title">密码:</view>
				<input name="password" type="password" placeholder="请输入密码"></input>
			</view>
			<view class="padding flex flex-direction">
				<button class="cu-btn bg-green shadow-blur round lg" formType="submit">注 册</button>
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
				let formdata = e.detail.value
				debugger
				// uni.showModal({
				// 	content: '表单数据内容：' + JSON.stringify(formdata),
				// 	showCancel: false
				// });
				this.onAppReg(formdata)
			},
			formReset: function(e) {
				console.log('清空数据')
				
			},
			onAppReg(formdata) {
				util.request(api.AuthRegByApp, JSON.stringify(formdata), 'POST').then(res => {
					if(res.success){
						setTimeout(function() {
							uni.showToast({
								title:"注册成功"
							})
						}, 0);
						uni.navigateBack()
					}
					
				});
			},
		}
	}
</script>

<style>
</style>
