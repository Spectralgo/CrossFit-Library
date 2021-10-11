import Axios from "axios";

const initState = () => ({
  message: ""
});

export const state = initState;

export const mutations = {
  setMessage(state, message){
    state.message =  message
  },
  reset(state){
    Object.assign(state, initState())
  }
}

export const actions = {
  async nuxtServerInit({commit, dispatch}){
    const message = (await Axios.get("http://localhost:5000/api/home")).data;
    console.log("api called");
    commit("setMessage", message);
    dispatch("tricks/fetchTricks");
  },


}
