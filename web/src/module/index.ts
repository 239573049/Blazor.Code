import { Provider, Injectable } from '@opensumi/di';
import { BrowserModule } from '@opensumi/ide-core-browser';
import { SampleContribution } from './index.contribution';

if(window.location.search){
  // TODO:如果添加参数webassembly无法加载
  window.localStorage.setItem('search',window.location.search)
  if(window.location.href !== window.location.origin+'/'){
    window.location.href = window.location.origin+'/'
  }
}

@Injectable()
export class SampleModule extends BrowserModule {
  providers: Provider[] = [
    SampleContribution,
  ];
}
