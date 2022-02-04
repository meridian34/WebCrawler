<template>
  <div>
    <!-- input component -->
    <crawling-component></crawling-component>
    
    <!-- loading animation -->
    <loading-component ></loading-component>

    <!-- table block -->
    <tests-table-component v-if="testsComponentVisible"></tests-table-component>
  </div>
</template>

<script>

import loadingComponent from '../components/Loading.vue'
import crawlingComponent from '../components/CrawlingForm.vue'
import testsTableComponent from '../components/TestsTable.vue'
import {eventEmitter} from '../main.js'

export default {
  data(){
    return{
      testsComponentVisible: true
    }
  },

  components:{
    'loadingComponent' : loadingComponent,
    'crawlingComponent' : crawlingComponent,
    'testsTableComponent' : testsTableComponent
  },
  created: function(){
    eventEmitter.$on("testsLoadingError",()=>{
          this.testsComponentVisible = false;
      })
  }
};
</script>

<style>

</style>