<template>
  <div>
    <usrls-table-component v-bind:page="pageUniqueSitemapUrl"></usrls-table-component>

    <usrls-table-component v-bind:page="pageUniqueWebsiteUrl"></usrls-table-component>

    <perfomance-table-component v-bind:page="page"></perfomance-table-component>
  </div>
</template>

<script>
import usrlsTableComponent from '../components/urlsTable.vue'
import perfomanceTableComponent from '../components/PerfomenceTable.vue'

export default {
  data() {
    return {
      page:{},
      pageUniqueSitemapUrl:{
            title: "URLs NOT FOUND AT WEBSITE",
            urlList: []
          },
      pageUniqueWebsiteUrl:{
            title: "URLs NOT FOUND AT SITEMAP",
            urlList: []
          },
    }
  },

  methods:{
    
    setUniqueSitemapUrl(itemPage){
          let listLinks = itemPage.links.filter(function (item){
            let buf = (item.fromSitemap===true && item.fromHtml === false)            
            return buf
          })

          this.pageUniqueSitemapUrl = {
            title: "URLs NOT FOUND AT WEBSITE",
            urlList: listLinks
          }
    },

    setUniqueWebsiteUrl(itemPage){
          let listLinks = itemPage.links.filter(function (item){
            let buf = (item.fromSitemap===false && item.fromHtml === true)            
            return buf
          })

          this.pageUniqueWebsiteUrl = {
            title: "URLs NOT FOUND AT WEBSITE",
            urlList: listLinks
          }
    },

    initData() {
      
      let {testId} = this.$router.currentRoute.query;
      this.resource.get({testId})
        .then((response) => {
          return response.json();
        })
        .then((body) => {
          this.page = body.detailsPage;
          this.setUniqueSitemapUrl(body.detailsPage)
          this.setUniqueWebsiteUrl(body.detailsPage)
          
        });
    }
  },
created: function () {
  this.resource = this.$resource('api/v1/TestDetails')
  this.initData();
  
  },
  components:{
    'usrlsTableComponent' : usrlsTableComponent,
    'perfomanceTableComponent': perfomanceTableComponent
  },
};

  
</script>

<style>
</style>