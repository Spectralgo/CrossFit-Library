<template><svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 48 48" :width="width" :height="height"><title>{{title}}</title><g :fill="_secondaryfill"><path d="M45,9H41V28a4,4,0,0,1-4,4H24.961l-7,5H25.7l11.748,7.832A1,1,0,0,0,39,44V37h6a2,2,0,0,0,2-2V11A2,2,0,0,0,45,9Z"></path><path d="M37,3H3A2,2,0,0,0,1,5V28a2,2,0,0,0,2,2H9v9a1,1,0,0,0,1.581.814L24.32,30H37a2,2,0,0,0,2-2V5A2,2,0,0,0,37,3Z" :fill="fill"></path></g></svg></template>

<script>
export default {
	props: {
		width: {
			type: [Number, String],
			default: "100%"
		},
		height: {
			type: [Number, String],
			default: "100%"
		},
		strokewidth: {
			type: [Number, String],
			default: 1
		},
		title: {
			type: String,
			default: "comments"
		},
		fill: {
			type: String,
			default: "currentColor"
		},
		secondaryfill: {
			type: String
		}
	},
	computed: {
		_secondaryfill: function () {
			return this.secondaryfill || this.fill
		}
	}
}
</script>