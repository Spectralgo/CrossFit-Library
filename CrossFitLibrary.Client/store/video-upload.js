
const initState = () => ({
  uploadPromise: null,
  uploadCancelSource: null,
  uploadCompleted: false,
  active: false,
  component: null,
});

export const state = initState;

export const mutations = {
  activate(state, {component}){
    state.active = true;
    state.component = component;
  },
  hide(state){
   state.active = false;
  },
  setTask(state, {uploadPromise , source}) {
    state.uploadPromise = uploadPromise
    state.uploadCancelSource = source
  },
  completeUpload(state) {
    state.uploadCompleted = true;
  },
  reset(state) {
    Object.assign(state, initState())
  }
}

export const actions = {
  async startUploadingVideo({commit, dispatch}, {form}) {
    // we need to monitor if the upload is completed
    // the source object contains a token that we can use to cancel the upload
    const source = this.$axios.CancelToken.source()
    const uploadPromise = this.$axios.post("/api/video", form, {
      cancelToken: source.token,
      progress: false}
    ).then(({data}) => {
      commit('completeUpload')
      console.log(data)
      // we are returning the data because we need to know if the video was uploaded
      return data;
    }).catch(err => {
      if (this.$axios.isCancel(err)) {
        // todo: popup a message to the user to let them know that the upload was cancelled
      }
    })

    // we're going to cancel the upload if the user closes the dialog
    commit("setTask", {uploadPromise, source})
  },
  async cancelUpload({commit, state}) {
    if (state.uploadPromise) {
      if (state.uploadCompleted){
        commit('hide')
        const videoFileName = await state.uploadPromise
        await this.$axios.delete("/api/video/" + videoFileName );
      }else{
        state.uploadCancelSource.cancel()
      }
    }
    commit("reset")
  },
  async createSubmission({commit, dispatch, state}, {form}) {
    form.videoFileName= await state.uploadPromise
    await dispatch('submissions/createSubmission', {form}, {root:true})
    commit('reset')
  }
}
