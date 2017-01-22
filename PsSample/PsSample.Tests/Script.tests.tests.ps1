Describe "Script" {
	Context "Exists" {
		It "SayHello Runs" {
			SayHello | Should be 'hello'
		}
	}
}