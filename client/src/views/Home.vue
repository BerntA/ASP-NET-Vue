<template>
  <v-container fluid>
    <v-card flat>
        <template v-slot:text>
        <v-text-field
          v-model="search"
          label="Search"
          prepend-inner-icon="mdi-magnify"
          variant="outlined"
          hide-details
          single-line
        ></v-text-field>
      </template>

      <v-data-table :headers="headers" :items="items" item-key="id" :loading="loading" :items-per-page="-1"
        :search="search">
        <template v-slot:top>
          <v-toolbar flat>
            <v-toolbar-title>Contact List</v-toolbar-title>

            <v-dialog v-model="dialog" max-width="500px">
              <template v-slot:activator="{ props }">
                <v-btn class="mb-2" color="primary" v-bind="props">
                  Add Contact
                </v-btn>
              </template>

              <v-card>
                <v-card-title>
                  <span class="text-h5">{{ formTitle }}</span>
                </v-card-title>

                <v-card-text>
                  <v-container>
                    <v-row>
                      <v-col cols="12">
                        <v-text-field v-model="editedItem.name" label="Name"></v-text-field>
                      </v-col>
                      <v-col cols="12">
                        <v-text-field v-model="editedItem.phone" label="Phone"></v-text-field>
                      </v-col>
                      <v-col cols="12">
                        <v-text-field v-model="editedItem.address" label="Address"></v-text-field>
                      </v-col>
                    </v-row>
                  </v-container>
                </v-card-text>

                <v-card-actions>
                  <v-spacer></v-spacer>
                  <v-btn color="blue-darken-1" variant="text" @click="close">
                    Cancel
                  </v-btn>
                  <v-btn color="blue-darken-1" variant="text" @click="save">
                    Save
                  </v-btn>
                </v-card-actions>
              </v-card>
            </v-dialog>

            <v-dialog v-model="dialogDelete" max-width="500px">
              <v-card>
                <v-card-title class="text-h5">Are you sure?</v-card-title>
                <v-card-actions>
                  <v-spacer></v-spacer>
                  <v-btn color="blue-darken-1" variant="text" @click="closeDelete">Cancel</v-btn>
                  <v-btn color="blue-darken-1" variant="text" @click="deleteItemConfirm">OK</v-btn>
                  <v-spacer></v-spacer>
                </v-card-actions>
              </v-card>
            </v-dialog>
          </v-toolbar>
        </template>
        <template v-slot:item.createdAt="{ item }">
          {{ this.$moment(item.createdAt).format('DD.MM.YYYY HH:mm:ss') }}
        </template>
        <template v-slot:item.updatedAt="{ item }">
          {{ this.$moment(item.updatedAt).format('DD.MM.YYYY HH:mm:ss') }}
        </template>
        <template v-slot:item.actions="{ item }">
          <v-icon class="me-2" size="small" title="Edit" @click="editItem(item)">
            mdi-pencil
          </v-icon>
          <v-icon size="small" title="Delete" @click="deleteItem(item)">
            mdi-delete
          </v-icon>
        </template>
      </v-data-table>
    </v-card>
  </v-container>
</template>

<script>
export default {
  name: "ContactListApp",
  async mounted() {
    await this.fetchContactList()
  },
  data() {
    return {
      items: [],
      search: '',
      loading: false,
      dialog: false,
      dialogDelete: false,
      headers: [
        { title: 'Name', key: 'name', sortable: true },
        { title: 'Phone Number', key: 'phone', sortable: true },
        { title: 'Address', key: 'address', sortable: true },
        { title: 'Created', key: 'createdAt', sortable: true },
        { title: 'Updated', key: 'updatedAt', sortable: true },
        { title: '', key: 'actions', sortable: false },
      ],
      editedIndex: -1,
      editedItem: {
        name: '',
        phone: '',
        address: '',
        id: null,
      },
      defaultItem: {
        name: '',
        phone: '',
        address: '',
        id: null,
      },
    }
  },
  methods: {
    async fetchContactList() {
      this.loading = true
      try {
        let response = await this.$http('/api/contacts')
        this.items = response.data
      } catch (e) {
        console.error(e)
      } finally {
        this.loading = false
      }
    },
    editItem(item) {
      this.editedIndex = this.items.indexOf(item)
      this.editedItem = Object.assign({}, item)
      this.dialog = true
    },
    deleteItem(item) {
      this.editedIndex = this.items.indexOf(item)
      this.editedItem = Object.assign({}, item)
      this.dialogDelete = true
    },
    async deleteItemConfirm() {
      await this.$http.delete(`/api/contacts?id=${this.items[this.editedIndex].id}`)
        .then(() => {
          this.items.splice(this.editedIndex, 1)
        })
        .catch(error => {
          console.error(error)
        })
      this.closeDelete()
    },
    close() {
      this.dialog = false
      this.$nextTick(() => {
        this.editedItem = Object.assign({}, this.defaultItem)
        this.editedIndex = -1
      })
    },
    closeDelete() {
      this.dialogDelete = false
      this.$nextTick(() => {
        this.editedItem = Object.assign({}, this.defaultItem)
        this.editedIndex = -1
      })
    },
    async save() {
      if (this.editedIndex > -1) {
        await this.$http.post('/api/contacts', { ...this.editedItem, ...{ id: this.items[this.editedIndex].id } })
          .then(response => {
            Object.assign(this.items.find(o => o.id == response.data.id), response.data)
          })
          .catch(error => {
            console.error(error)
          })
      } else {
        await this.$http.put('/api/contacts', this.editedItem)
          .then(() => {
            this.items.push(this.editedItem)
          })
          .catch(error => {
            console.error(error)
          })
      }
      this.close()
    },
  },
  computed: {
    formTitle() {
      return this.editedIndex === -1 ? 'New Contact' : 'Edit Contact'
    },
  },
  watch: {
    dialog(val) {
      val || this.close()
    },
    dialogDelete(val) {
      val || this.closeDelete()
    },
  },
}
</script>
