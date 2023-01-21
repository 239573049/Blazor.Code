// @ts-nocheck
import * as React from 'react';

// 对应razor组件

/**
 * 用于渲染Blazor组件
 */
class RenderView extends React.Component {
  render() {
    // 对应razor组件
    return (
      <render-razor></render-razor>
    )
  }
}

/**
 * 显示全局Using引用 
 */
class UsingList extends React.Component {
  render() {
    return (<global-using></global-using>)
  }
}

class MasaList extends React.Component{
  render() {
    return(<masa-list></masa-list>)
  }
}

class AssemblyLoad extends React.Component{
  render(): React.ReactNode {
    return (<assembly-load></assembly-load>)
  }
}

class BootstrapList extends React.Component{
  render(): React.ReactNode {
    return(<bootstrap-list></bootstrap-list>)
  }
}

export {
  RenderView,
  UsingList,
  MasaList,
  BootstrapList,
  AssemblyLoad
}