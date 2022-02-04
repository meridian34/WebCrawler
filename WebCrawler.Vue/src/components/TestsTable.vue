<template>
  <div>
      <h2 class="elementInTheCenter mt-50">Test results</h2>
    <table class="table table-striped table-bordered table-active">
      <thead class="thead-dark">
        <tr >
          <th>Url</th>
          <th>Test Date Time</th>
          <th></th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="test in page.tests" :key="test.id">
          <td>{{ test.userLink }}</td>
          <td>{{ formatData(test.testDateTime) }}</td>
          <td>
              <router-link :to="'/TestDetails?testId='+test.id">
              <u>See details</u>
              </router-link>
            </td>
        </tr>
      </tbody>
    </table>

    <paging-component 
    v-bind:currentPage="page.currentPage"
    v-bind:itemsCount="page.itemsCount"
    v-bind:totalPages="page.totalPages"
    @changePageEvent="changeTestPageHandler($event)" ></paging-component>
  </div>

</template>

<script>
const defaultPageNumber = 1;
const defaultPageSize = 5;
import {eventEmitter} from '../main.js'
import pagingComponent from '../components/Paging.vue'

export default {
data() {
    return {
      page: {}
    };
  },
  methods: {
    changeTestPageHandler({pageNumber, pageSize}){
        this.changeTestPage(pageNumber, pageSize)
    },

    changeTestPage(pageNumber, pageSize){
        this.resource.get({pageNumber, pageSize})
        .then((response) => {
          return response.json();
        })
        .then((body) => {
          this.page = body.testsPage;
        });
    },

    formatData(datetime) {
      let dt = new Date(datetime);
      //dd-MM-yyyy HH:mm:ss
      return dt.getUTCFullYear() +
        "-" +
        ("0" + (dt.getUTCMonth() + 1)).slice(-2) +
        "-" +
        ("0" + dt.getUTCDate()).slice(-2) +
        " " +
        ("0" + dt.getUTCHours()).slice(-2) +
        ":" +
        ("0" + dt.getUTCMinutes()).slice(-2) +
        ":" +
        ("0" + dt.getUTCSeconds()).slice(-2);
    },
  },
  created: function () {
    
    this.resource = this.$resource('api/v1/Tests')

    this.changeTestPage(defaultPageNumber, defaultPageSize);

    eventEmitter.$on("crawlingFinished",()=>{
          this.changeTestPage(defaultPageNumber, defaultPageSize)
      })

    eventEmitter.$on("changeTestsPage",({pageNumber, pageSize})=>{
          this.changeTestPage(pageNumber, pageSize)
      })
  },

  components:{
      "pagingComponent" : pagingComponent
  },
}
</script>

<style>
.mt-50 {
  margin-top: 50px;
}
</style>