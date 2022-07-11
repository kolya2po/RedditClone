import { Component } from '@angular/core';
import {UserService} from "../../services/user.service";
import {SearchService} from "../../services/search.service";

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent {
  title = 'Eddit';
  search = '';
  constructor(public us: UserService,
              private ss: SearchService) { }

  ngDoCheck(): void {
    this.ss.changeSearch(this.search);
    console.log('sadasd');
  }
}
