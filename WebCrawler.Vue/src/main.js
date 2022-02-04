import Vue from 'vue'
import App from './App.vue'
import VueResource from 'vue-resource'
import VueRouter from 'vue-router'
import router from './routes'

export const eventEmitter = new Vue()


Vue.use(VueResource)
Vue.use(VueRouter)

Vue.http.options.root = 'http://localhost:5000/'

new Vue({
  el: '#app',
  render: h => h(App),
  router: router  
})

