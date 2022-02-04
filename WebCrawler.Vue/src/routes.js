import VueRouter from "vue-router";
import TestsPage from './pages/TestsPage'
import TestDetailsPage from './pages/TestDetailsPage'

 export default new VueRouter({
     routes:[
         {
             path:'',
             component: TestsPage
         },
         {
             path:'/TestDetails',
             component: TestDetailsPage
         },
         {
             path:'/TestDetails/:testId',
             component: TestDetailsPage
         }
     ],
     mode: 'history'
 })
