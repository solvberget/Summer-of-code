# How to contribute

Here are a few guidelines that everyone should follow so that we have a chance of keeping on top of things.

## Making Changes

1. [Fork](http://help.github.com/forking/) the repository on GitHub
1. Clone your fork locally
1. Configure the upstream repo (`git remote add upstream git://github.com/capgemini-stavanger/Solvberget`)
1. Create a local branch (`git checkout -b feature/awesome-feature`)
1. Work on your feature
1. Rebase if required (see below)
1. Push the branch up to GitHub (`git push origin feature/awesome-feature`)
1. Send a Pull Request on GitHub

You should **never** work on a clone of master, and you should **never** send a pull request from master - always from a feature branch. The reasons for this are detailed below.

## Handling Updates from Upstream/Master

While you're working away in your branch it's quite possible that your upstream master (most likely the canonical Solvberget version) may be updated. If this happens you should:

1. [Stash](http://progit.org/book/ch6-3.html) any un-committed changes you need to
1. `git checkout master`
1. `git pull upstream master`
1. `git checkout feature/awesome-feature`
1. `git rebase master feature/awesome-feature`
1. `git push origin master` - (optional) this this makes sure your remote master is up to date

This ensures that your history is "clean" i.e. you have one branch off from master followed by your changes in a straight line. Failing to do this ends up with several "messy" merges in your history, which we don't want. This is the reason why you should always work in a branch and you should never be working in, or sending pull requests from, master.

If you're working on a long running feature then you may want to do this quite often, rather than run the risk of potential merge issues further down the line.

## Sending a Pull Request

While working on your feature you may well create several branches, which is fine, but before you send a pull request you should ensure that you have rebased back to a single "Feature branch". We care about your commits, and we care about your feature branch; but we don't care about how many or which branches you created while you were working on it :smile:.

When you're ready to go you should confirm that you are up to date and rebased with upstream/master (see "Handling Updates from Upstream/Master" above), and then:

1. `git push origin feature/awesome-feature`
1. Send a descriptive [Pull Request](http://help.github.com/pull-requests/) on GitHub - making sure you have selected the correct branch in the GitHub UI!
1. Pling someone to review your changes and merge them into master (this can be done from GitHub). Please don't merge your own PRs :wink:.
