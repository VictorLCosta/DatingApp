import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { ListComponent } from './list/list.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { MessagesComponent } from './messages/messages.component';

const routes: Routes = [
  {path: "", component: HomeComponent},
  {path: "members", component: MemberListComponent},
  {path: "members/:id", component: HomeComponent},
  {path: "lists", component: ListComponent},
  {path: "messages", component: MessagesComponent},
  {path: "**", component: HomeComponent, pathMatch: "full"}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
